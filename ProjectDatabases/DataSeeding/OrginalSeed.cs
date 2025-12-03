using Microsoft.EntityFrameworkCore;

namespace ProjectDatabases.DataSeeding;

public static class OrginalSeed
{
    public static async Task SeedAsync()
    {
        using var db = new StoreContext();
        await db.Database.MigrateAsync();

        if (!await db.Customers.AnyAsync())
        {
            db.Customers.AddRange(
                new Customer{CustomerName = "Lasse", CustomerEmail = "Lasse@hotmail.com", CustomerAddress = "Alhagsvägen 3" },
                new Customer{CustomerName = "Alex", CustomerEmail = "Alex@hotmail.com", CustomerAddress = "Friggsvägen 3"}
                );
            await db.SaveChangesAsync();
            Console.WriteLine("Seeded Customers Db");
        }
        
        if (!await db.Categories.AnyAsync())
        {
            db.Categories.AddRange(
                new Category {CategoryId = 1, CategoryName = "Phone", Description = "Smartphones and mobile devices"},
                new Category {CategoryId = 2,CategoryName = "Tv", Description = "4K UHD televisions and smart TVs" }
            );
            await db.SaveChangesAsync();
            Console.WriteLine("Seeded Categories Db");
        }

        if (!await db.Products.AnyAsync())
        {
            db.Products.AddRange(
                new Product { CategoryId = 1, ProductName = "Iphone-16", ProductPrice = 8500},
                new Product { CategoryId = 2, ProductName = "Luxor 55 - LX5540UHD", ProductPrice = 12500}
            );
            await db.SaveChangesAsync();
            Console.WriteLine("Seeded Products Db");
        }

        
    }
    
    
}