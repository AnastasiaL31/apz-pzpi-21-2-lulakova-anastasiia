using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartShelter_Web.Areas.Identity.Data;

namespace SmartShelter_Web.Data;

public class SmartShelterDBContext : IdentityDbContext<ApplicationUser>
{
    public SmartShelterDBContext(DbContextOptions<SmartShelterDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
