namespace ProjectDatabases.Services;

/// <summary>
/// Service class responsible for handling product-related database operations,
/// including CRUD functionality and summary view listings.
/// </summary>
public class ProductService
{
    /// <summary>
    /// Displays a list of all products stored in the database.
    /// </summary>
    public static async Task ListProductAsync()
    {
        using var db = new StoreContext();
        
        // Retrieve all products without tracking improves performance for read-only operations
        var products = await db.Products
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        
        Console.WriteLine("\n=== Products ===");
        Console.WriteLine("ProductId | ProductName | ProductPrice | CategoryId");
        
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice} | {product.CategoryId}");
        }
    }
    
    /// <summary>
    /// Adds a new product to the database.
    /// Validates user input and ensures the provided CategoryId exists.
    /// </summary>
    public static async Task AddProductAsync()
    {
        using var db = new StoreContext();
     
        // Display existing products before adding a new one
        var rows = await db.Products
            .AsNoTracking()
            .OrderBy(product => product.ProductId)
            .ToListAsync();

        Console.WriteLine("ProductId | Name | Price | CategoryId");
        foreach (var row in rows)
        {
            Console.WriteLine($"{row.ProductId} | {row.ProductName} | {row.ProductPrice} | {row.CategoryId}");
        }
    
        // Product name input
        Console.WriteLine("Enter product name:");
        var productName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(productName) || productName.Length > 100)
        {
            Console.WriteLine("Product name is required (max 100 characters).");
            return;
        }
        
        // Product price input
        Console.WriteLine("Enter price:");
        if (!decimal.TryParse(Console.ReadLine(), out var productPrice))
        {
            Console.WriteLine("Invalid price.");
            return;
        }
     
        // CategoryId input
        Console.WriteLine("Enter CategoryId:");
        if (!int.TryParse(Console.ReadLine(), out var categoryId))
        {
            Console.WriteLine("Invalid CategoryId.");
            return;
        }

        // Ensure category exists
        var categoryExists = await db.Categories
            .AnyAsync(c => c.CategoryId == categoryId);
        if (!categoryExists)
        {
            Console.WriteLine("CategoryId does not exist.");
            return;
        }
        
        // Add new product
        db.Products.Add(new Product
        {
            ProductName = productName,
            ProductPrice = productPrice,
            CategoryId = categoryId
        });

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product added successfully!");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine("Database error: " + exception.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Edits an existing product based on its ID.
    /// The user may choose to update the name, price, and category.
    /// </summary>
    public static async Task EditProductAsync(int productId)
    {
        using var db = new StoreContext();
        
        // Fetch the product
        var product = await db.Products
            .FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        // Update product name
        Console.WriteLine($"{product.ProductName}");
        var productName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(productName))
        {
            product.ProductName = productName;
        }
        
        // Update product price
        Console.WriteLine($"Current Product Price: {product.ProductPrice}");
        var priceInput = Console.ReadLine()?.Trim();
        if (decimal.TryParse(priceInput, out var productPrice))
        {
            product.ProductPrice = productPrice;
        }
        
        // Update product category
        Console.WriteLine($"Current Product CategoryId: {product.CategoryId}");
        var categoryInput = Console.ReadLine()?.Trim();

        if (int.TryParse(categoryInput, out var categoryId))
        {
            var categoryExists = await db
                .Categories
                .AnyAsync(c => c.CategoryId == categoryId);
            if (categoryExists)
            {
                product.CategoryId = categoryId;
            }
            else
            {
                Console.WriteLine("Invalid CategoryId. Keeping previous value.");
            }
        }

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product updated successfully!");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine("Database error: " + exception.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Deletes a product from the database based on its ID.
    /// </summary>
    public static async Task DeleteProductAsync(int productId)
    {
        using var db = new StoreContext();

        var product = await db
            .Products
            .FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        db.Products.Remove(product);

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Product deleted successfully!");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine("Database error: " + exception.GetBaseException().Message);
        }
    }

    /// <summary>
    /// Displays summarized sales information from the ProductSalesViews database view.
    /// </summary>
    public static async Task ProductSalesViews()
    {
        using var db = new StoreContext();
        
        var products = await db.ProductSalesViews
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .ToListAsync();

        Console.WriteLine("== Product Summary ==");
        Console.WriteLine("ProductId | ProductName | TotalQuantitySold");

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.TotalQuantitySold}");
        }
    }
}
