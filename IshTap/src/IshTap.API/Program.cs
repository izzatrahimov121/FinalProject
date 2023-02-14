#region USING
using IshTap.Business.Helpers;
using IshTap.Business.HelperServices.Implementations;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Mappers;
using IshTap.Business.Services.Implementations;
using IshTap.Business.Services.Interfaces;
using IshTap.Core.Entities;
using IshTap.DataAccess.Contexts;
using IshTap.DataAccess.Repository.Implementations;
using IshTap.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using T = IshTap.Business.HelperServices.Implementations;
#endregion


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(VacancieMapper).Assembly);


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddDbContext<AppDbContexts>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});



builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

})
    .AddEntityFrameworkStores<AppDbContexts>()
    .AddEntityFrameworkStores<AppDbContexts>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"])),
        LifetimeValidator = (_, expires, _, _) => expires != null ? expires > DateTime.UtcNow : false,
    };
});


builder.Services.AddAuthorization();


#region Add service and repository
//add repository
builder.Services.AddScoped<IVacancieRepository, VacancieRepository>();
builder.Services.AddScoped<ICVRepository, CVRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IJobTypeRepository, JobTypeRepository>();
builder.Services.AddScoped<IExperiencesRepository, ExperiencesRepository>();

//add service
builder.Services.AddScoped<IVacancieService, VacancieService>();
builder.Services.AddScoped<ICVService, CVService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IJobTypeService, JobTypeService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IEducationService, EducationService>();

//add helper service
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ITokenHandler, T.TokenHandler>();
builder.Services.AddSingleton<IFileService, FileService>();

#endregion


builder.Services.AddScoped<AppDbContextInitializer>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.RoleSeedAsync();
    await initializer.UserSeedAsync();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
