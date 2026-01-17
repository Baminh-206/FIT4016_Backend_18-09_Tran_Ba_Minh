using Bogus;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementApp;

public class OrderDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ===== CONSTRAINT =====
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name).IsUnique();

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Sku).IsUnique();

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderNumber).IsUnique();

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CustomerEmail).IsUnique();

        // ===== SEED PRODUCTS (15) =====
        var products = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName() + f.IndexFaker)
            .RuleFor(p => p.Sku, f => f.Commerce.Ean13())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 500))
            .RuleFor(p => p.StockQuantity, f => f.Random.Int(10, 100))
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .Generate(15);

        modelBuilder.Entity<Product>().HasData(products);

        // ===== SEED ORDERS (30) =====
        var orders = new Faker<Order>()
            .RuleFor(o => o.Id, f => f.IndexFaker + 1)
            .RuleFor(o => o.ProductId, f => f.Random.Int(1, 15))
            .RuleFor(o => o.OrderNumber,
                f => $"ORD-{DateTime.Now:yyyyMMdd}-{f.IndexFaker:0000}")
            .RuleFor(o => o.CustomerName, f => f.Name.FullName())
            .RuleFor(o => o.CustomerEmail, f => f.Internet.Email())
            .RuleFor(o => o.Quantity, f => f.Random.Int(1, 5))
            .RuleFor(o => o.OrderDate, f => f.Date.Past(1))
            .RuleFor(o => o.DeliveryDate, f => f.Random.Bool() ? f.Date.Future() : null)
            .Generate(30);

        modelBuilder.Entity<Order>().HasData(orders);
    }
}
