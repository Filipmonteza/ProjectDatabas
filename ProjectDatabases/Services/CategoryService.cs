using Microsoft.EntityFrameworkCore;

namespace ProjectDatabases.Services;

public class CategoryService
{
     
    public static async Task ListCategoryAsync()
    {
        using var db = new StoreContext();
        
        // List all Category
        var categories = await db.Categories
            .AsNoTracking()
            .OrderBy(c => c.CategoryId)
            .ToListAsync();
        
        Console.WriteLine("\n===Categories===");
        Console.WriteLine("CategoryId | CategoryName | Description");

        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId} | {category.CategoryName} | {category.Description}");
        }
    }

    /// <summary>
    /// Adding Category
    /// </summary>
    public static async Task AddCategoryAsync()
    {
        using var db = new StoreContext();
        
        Console.WriteLine("CategoryName: ");
        var categoryName = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(categoryName) || categoryName.Length > 50)
        {
            Console.WriteLine("CategoryName is required (Max 50 characters)");
            return;
        }
        
        Console.WriteLine("Description: ");
        var description = Console.ReadLine()?.Trim() ?? string.Empty;

        db.Categories.Add(new Category
        {
            CategoryName = categoryName,
            Description = description
        });
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Category Added");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
    
    public static async Task EditCategoryAsync(int id)
    {
        using var db = new StoreContext();

        var category = await db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null)
        {
            Console.WriteLine("Category not found");
            return;
        }
        
        // Show current value
        Console.WriteLine($"{category.CategoryName}");
        var categoryName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(categoryName))
        {
            category.CategoryName = categoryName;
        }
        
        Console.WriteLine($"{category.Description}");
        var categoryDescription = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(categoryDescription))
        {
            category.Description = categoryDescription;
        }

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Category Edited");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }
    }

    public static async Task DeleteCategoryAsync(int id)
    {
        using var db =  new StoreContext();
        var category = await db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null)
        {
            Console.WriteLine("Category not found");
            return;
        }
        
        db.Categories.Remove(category);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
