using TestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TestAPI.Data;
public class ShopContext : DbContext {

    public ShopContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(a => a.Category)
            .HasForeignKey(k => k.CategoryId);

        modelBuilder.Seed();
    }

    public DbSet<Product> Products {get; set;}

    public DbSet<Category> Categories {get; set;}
}