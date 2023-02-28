using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace IshTap.Business.HelperServices.Implementations;

public class MyBackgroundService : BackgroundService
{
    private readonly ILogger<MyBackgroundService> _logger;
    private readonly IServiceProvider _services;
    public MyBackgroundService(ILogger<MyBackgroundService> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("MyBackgroundService is running.");

                using (var scope = _services.CreateScope())
                {
                    //Vacancie
                    var _vacancieRepository = scope.ServiceProvider.GetRequiredService<IVacancieRepository>();
                    var vacancies = await _vacancieRepository.FindAll().Where(v => v.IsActive == true).ToListAsync();
                    foreach (var vacancie in vacancies)
                    {
                        if (DateTime.Now >= vacancie.ExpireOn)
                        {
                            vacancie.IsActive = false;
                            _vacancieRepository.Update(vacancie);
                        }
                    }
                    await _vacancieRepository.SaveAsync();

                    //CV
                    var _cvRepository = scope.ServiceProvider.GetRequiredService<ICVRepository>();
                    var cvs = await _cvRepository.FindAll().Where(c=>c.IsActive==true).ToListAsync();
                    foreach (var cv in cvs)
                    {
                        if (DateTime.Now>=cv.ExpireOn)
                        {
                            cv.IsActive = false;
                            _cvRepository.Update(cv);
                        }
                    }
                    await _cvRepository.SaveAsync();


                    await Task.Delay(TimeSpan.FromHours(6), stoppingToken); // 1 day delay
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating IsActive property.");
            }
        }
    }
}
