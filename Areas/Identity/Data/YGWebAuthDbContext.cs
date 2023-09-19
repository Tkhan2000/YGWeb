using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YGWeb.Areas.Identity.Data;

namespace YGWeb.Data;

public class YGWebAuthDbContext : IdentityDbContext<YGWebUser>
{
    public YGWebAuthDbContext(DbContextOptions<YGWebAuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<YGWebUser> Users {  get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
