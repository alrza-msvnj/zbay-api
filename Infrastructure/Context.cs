using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Shop> Shop { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<ShopCategory> ShopCategory { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasIndex(e => e.Email)
            .IsUnique();
            e.HasIndex(e => e.PhoneNumber)
            .IsUnique();

            e.HasOne(u => u.Shop)
            .WithOne(s => s.Owner)
            .HasForeignKey<User>(u => u.ShopId);
        });

        modelBuilder.Entity<Shop>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasIndex(e => e.InstagramId)
            .IsUnique();
            e.HasIndex(e => e.InstagramUrl)
            .IsUnique();
            e.HasIndex(e => e.Name)
            .IsUnique();
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasOne(p => p.Shop)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.ShopId);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasIndex(e => e.Title)
            .IsUnique();

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

        modelBuilder.Entity<ProductCategory>(e =>
        {
            e.HasKey(e => new { e.ProductId, e.CategoryId });

            e.HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId);

            e.HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(sc => sc.CategoryId);
        });
    }
}
