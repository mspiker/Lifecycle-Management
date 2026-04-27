using LifecycleManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Item>()
            .HasOne(i => i.Owner)
            .WithMany(u => u.OwnedItems)
            .HasForeignKey(i => i.OwnerUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Item>()
            .HasOne(i => i.ItemType)
            .WithMany(t => t.Items)
            .HasForeignKey(i => i.ItemTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
