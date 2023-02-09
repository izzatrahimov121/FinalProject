using IshTap.Business.HelperServices.Implementations;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Mappers;
using IshTap.Business.Services.Implementations;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Implementations;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContexts>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContexts>();


builder.Services.AddAutoMapper(typeof(VacancieMapper).Assembly);



//add repository
builder.Services.AddScoped<IVacancieRepository, VacancieRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IJobTypeRepository, JobTypeRepository>();
builder.Services.AddScoped<IExperiencesRepository, ExperiencesRepository>();

//add service
builder.Services.AddScoped<IVacancieService, VacancieService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IJobTypeService, JobTypeService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IEducationService, EducationService>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
