using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;

namespace ProjectDatabases.Services;

public class ProductService
{
    public static async Task ListProductAsync()
    {
        using var db = new StoreContext();
        
        var products = await db.Products
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        
        Console.WriteLine("\n=== Products ===");
        Console.WriteLine("ProductId | ProductName | ProductPrice | CategoryId");
        
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName}  | {product.ProductPrice} | {product.CategoryId}");
        }
    }
    
   public static async Task AddProductAsync()
    {
        // Ny context per operation (bra praxis)
        using var db = new StoreContext();
     
        // AsNoTracking = snabbare för read-Only scenarion. (ingen change tracking)
        var rows = await db.Products.AsNoTracking()
         .OrderBy(product => product.ProductId)
             .ToListAsync();
        Console.WriteLine("ProductId | Name | Pris | CategoryId");
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.ProductId} | {row.ProductName} | {row.ProductPrice} | {row.CategoryId}");
        }
    
     
        //product namn
        Console.WriteLine("Enter product name:");
        var productName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(productName) || productName.Length > 100)
        {
            Console.WriteLine("Name is required (max 100).");
            return;
        }
        
        //lägg till pris 
        Console.WriteLine("Enter Price");
        if (!decimal.TryParse(Console.ReadLine(), out var productPrice))
        {
            Console.WriteLine("Invalid Price.");
            return;
        }
     
        Console.WriteLine("Enter CategoryId:");
        if (!int.TryParse(Console.ReadLine(), out var categoryId))
        {
         Console.WriteLine("Invalid Category");
         return;
        }
        var categoryExist = await db.Categories.AnyAsync(c => c.CategoryId == categoryId);
        if (!categoryExist)
        {
         Console.WriteLine("CategoryId does not exist");
         return;
        }
        //Lägg till product
        db.Products.Add(new Product
            {
             ProductPrice = productPrice,
             ProductName = productName,
             CategoryId = categoryId
            }
        );
        try
        {
         await db.SaveChangesAsync();
         Console.WriteLine("Product added!");
        }
        catch (DbUpdateException exception)
        {
         Console.WriteLine("DB Error: " + exception.GetBaseException().Message);
        }
    }

    public static async Task EditProductAsync(int productId)
    {
        using var db = new StoreContext();
        
        var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }
        Console.WriteLine($"{product.ProductName}");
        var productName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(productName))
        {
            product.ProductName = productName;
        }
        
        Console.WriteLine($"Current Product price: {product.ProductPrice}");
        var priceInput = Console.ReadLine()?.Trim();
        if (decimal.TryParse(priceInput, out var productPrice))
        {
            product.ProductPrice = productPrice;
        }
        
        Console.WriteLine($"Current Product Category Id: {product.CategoryId}");
        var categoryInput = Console.ReadLine()?.Trim();
        if (int.TryParse(categoryInput, out var categoryId))
        {
            var categoryExist = await db.Categories.AnyAsync(c => c.CategoryId == categoryId);
            if (categoryExist)
            {
                product.CategoryId = categoryId;
            }
            else
            {
                Console.WriteLine("Invalid Category. Keep old value.");
            }
        }

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product updated!");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine("DB Error: " + exception.GetBaseException().Message);
        }
    }

    public static async Task DeleteProductAsync(int productId)
    {
        using var db = new StoreContext();

        var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }
        db.Products.Remove(product);
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product deleted!");
        }
        catch(DbUpdateException exception)
        {
            Console.WriteLine("DB Error: " + exception.GetBaseException().Message);
        }
    }
}
