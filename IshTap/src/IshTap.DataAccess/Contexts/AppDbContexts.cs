using IshTap.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IshTap.DataAccess.Contexts;

public class AppDbContexts : IdentityDbContext <AppUser>
{
    public AppDbContexts(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<JobType> JobTypes { get; set; }
    public DbSet<Vacancie> Vacancies { get; set; }
}
