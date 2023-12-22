using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using ThreadAlert.DTOs;
using ThreadAlert.Entities;

namespace ThreadAlert.Persistance;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Message> Messages { get; set; }
    public DbSet<DangerousObject> DangerousObjects { get; set; }
    public DbSet<HubConnection> HubConnections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Message>()
        .ToTable(tb => tb.HasTrigger("SomeTrigger"));

        base.OnModelCreating(builder);
    }
}
