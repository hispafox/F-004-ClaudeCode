using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Ignore(o => o.Total);

        modelBuilder.Entity<OrderItem>()
            .Ignore(i => i.LineTotal)
            .Property(i => i.UnitPrice).HasPrecision(18, 2);

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Acme", Email = "billing@acme.test" },
            new Customer { Id = 2, Name = "Globex", Email = "ops@globex.test" });
    }
}
