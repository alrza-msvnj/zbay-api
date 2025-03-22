using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Shop> Shop { get; set; }
    public DbSet<Category> Category { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasOne(u => u.Shop)
            .WithOne(s => s.Owner)
            .HasForeignKey<User>(u => u.ShopId);
        });

        modelBuilder.Entity<Shop>(e =>
        {
            e.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ShopCategory>(e =>
        {
            e.HasKey(e => new { e.ShopId, e.CategoryId });

            e.HasOne(sc => sc.Shop)
            .WithMany(s => s.ShopCategories)
            .HasForeignKey(sc => sc.ShopId);

            e.HasOne(sc => sc.Category)
            .WithMany(c => c.ShopCategories)
            .HasForeignKey(sc => sc.CategoryId);
        });
    }
}
