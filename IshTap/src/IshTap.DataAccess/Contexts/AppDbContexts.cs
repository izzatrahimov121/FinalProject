using IshTap.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IshTap.DataAccess.Contexts;

public class AppDbContexts : IdentityDbContext<AppUser>
{
    public AppDbContexts(DbContextOptions options) : base(options)
    {
    }


    public DbSet<Category> Categories { get; set; }

    //Vacancies
    public DbSet<JobType> JobTypes { get; set; }
    public DbSet<Vacancie> Vacancies { get; set; }

    //CVs
    public DbSet<CVs> CVs { get; set; }
    public DbSet<Educations> Educations { get; set; }
    public DbSet<Experiences> Experiences { get; set; }

    //Contacts
    public DbSet<GetInTouch> GetInTouch { get; set; }

    //fovarites
    public DbSet<FavoriteVacancies> FavoriteVacancies { get; set; }

    public DbSet<ApplyJob> ApplyJob { get; set; }
}
