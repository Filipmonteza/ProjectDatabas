using Microsoft.EntityFrameworkCore;
using ProjectDatabases.Models;

namespace ProjectDatabases;

public class StoreContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderRow> OrderRows => Set<OrderRow>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
   
    
    // View OrderSummary/CustomerOrderCountView/ProductSalesView - Keyless
    public DbSet<OrderSummary> OrderSummaries => Set<OrderSummary>();
    public DbSet<CustomerOrderCountView> CustomerOrderCountViews=> Set<CustomerOrderCountView>();
    public DbSet<ProductSalesView>  ProductSalesViews => Set<ProductSalesView>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "Shop.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        // ProductSalesView
        modelBuilder.Entity<ProductSalesView>(p =>
        {
            p.HasNoKey();
            p.ToView("ProductSalesView");
        });
        
        // OrderSummary View
        modelBuilder.Entity<OrderSummary>(o =>
        {
            o.HasNoKey(); // saknar PK alltså har ingen primärnyckel
            o.ToView("OrderSummaryView"); // kopplar tabellen mot SQLite
        });
        
        // CustomerOrderCountViews
        modelBuilder.Entity<CustomerOrderCountView>(c =>
        {   
            c.HasNoKey(); // saknar PK alltså har ingen primärnyckel
            c.ToView("CustomerOrderCountView"); // kopplar tabellen mot SQLite
        });
        
        modelBuilder.Entity<Customer>(c =>
        {
            // Prime-Key
            c.HasKey(x => x.CustomerId);
            
            // Properties
            c.Property(x=> x.CustomerName).HasMaxLength(50);
            c.Property(x => x.CustomerAddress).HasMaxLength(50);
            c.Property(x => x.CustomerEmail).HasMaxLength(50);
            
            // UNIQUE
            c.HasIndex(x => x.CustomerEmail).IsUnique();

        });

        modelBuilder.Entity<Order>(o =>
        {   
            //Prime-Key
            o.HasKey(x => x.OrderId);
            
            //Properties
            o.Property(x => x.OrderDate);
            o.Property(x => x.OrderTotalPrice).IsRequired();
            o.Property(x => x.OrderStatus).IsRequired().HasMaxLength(50);
            
            // Foreign-Key
            o.HasOne(x=>x.Customer)
                .WithMany(x=>x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderRow>(o =>
        {
            // Prime-Key
            o.HasKey(x => x.OrderRowId);
            
            // Properties
            o.Property(x => x.OrderRowUnitPrice).IsRequired();
            o.Property(x => x.OrderRowQuantity).IsRequired();
            
            // Foreign-Key
            o.HasOne(x => x.Order)
                .WithMany(x => x.OrderRows)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Foreign-Key
            o.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
       
        modelBuilder.Entity<Product>(p =>
        {
            // Prime-Key
            p.HasKey(x => x.ProductId);
            
            // Properties
            p.Property(x => x.ProductName).HasMaxLength(50).IsRequired();
            p.Property(x => x.ProductPrice).IsRequired();
            
            // Relation
            p.HasOne(x => x.Category)
                .WithMany(x=>x.Products).HasForeignKey(y=>y.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(c => 
        {
            // Prime-Key
            c.HasKey(x => x.CategoryId);
            
            c.Property(x => x.CategoryName).HasMaxLength(50).IsRequired();
            c.Property(x => x.Description).HasMaxLength(50);
        });
        
       
        



    }
}