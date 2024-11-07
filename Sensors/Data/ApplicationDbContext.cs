using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sensors.Data;

public class ApplicationDbContext : IdentityDbContext
{
    /// <summary>
    /// ClientSecrets table
    /// </summary>
    public DbSet<ClientSecret> ClientSecrets { get; set; }

    /// <summary>
    /// Groups table
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    /// Zones table
    /// </summary>
    public DbSet<Zone> Zones { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Group>(entity =>
        {
            // Configure foreign key relationship
            entity.HasOne(e => e.User) // Each Group belongs to one User
                  .WithMany() // Users can have many Groups
                  .HasForeignKey(e => e.UserId) // UserId is the foreign key
                  .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            // Configure foreign key relationship
            entity.HasOne(e => e.Group) // Each Zone belongs to one Group
                  .WithMany() // Groups can have many Zones
                  .HasForeignKey(e => e.GroupId) // GroupId is the foreign key
                  .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior
        });
    }
}