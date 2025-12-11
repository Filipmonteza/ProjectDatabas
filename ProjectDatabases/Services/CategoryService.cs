namespace ProjectDatabases.Services;

public class CategoryService
{
    // Lists all categories from the database and prints them in a formatted table.
    public static async Task ListCategoryAsync()
    {
        using var db = new StoreContext();
        
        //  Retrieve all categories without tracking to improve performance.
        var categories = await db.Categories
            .AsNoTracking()
            .OrderBy(c => c.CategoryId)
            .ToListAsync();
        
        Console.WriteLine("\n===Categories===");
        Console.WriteLine("CategoryId | CategoryName | Description");
        
        // Print each category in the list
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId} | {category.CategoryName} | {category.Description}");
        }
    }

    /// <summary>
    /// Adds a new category to the database.
    /// </summary>
    public static async Task AddCategoryAsync()
    {
        using var db = new StoreContext();
        
        Console.WriteLine("CategoryName: ");
        var categoryName = Console.ReadLine()?.Trim() ?? string.Empty;
        
        // Validate CategoryName input
        if (string.IsNullOrEmpty(categoryName) || categoryName.Length > 50)
        {
            Console.WriteLine("CategoryName is required (Max 50 characters)");
            return;
        }
        
        Console.WriteLine("Description: ");
        var description = Console.ReadLine()?.Trim() ?? string.Empty;
        
        // Create and add the new Category entity
        db.Categories.Add(new Category
        {
            CategoryName = categoryName,
            Description = description
        });
        try
        {
            // Persist changes to the database
            await db.SaveChangesAsync();
            Console.WriteLine("Category Added");
        }
        catch (DbUpdateException exception)
        {
            // Print database related error
            Console.WriteLine(exception.Message);
        }
    }
    
    /// <summary>
    /// Edits an existing category by Id
    /// </summary>
    /// <param name="id"></param>
    public static async Task EditCategoryAsync(int id)
    {
        using var db = new StoreContext();

        // Attempt to find the category with the specified Id
        var category = await db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null)
        {
            Console.WriteLine("Category not found");
            return;
        }
        
        // Show current CategoryName and accept new input
        Console.WriteLine($"{category.CategoryName}");
        var categoryName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(categoryName))
        {
            category.CategoryName = categoryName;
        }
        
        // show current description and accept new input
        Console.WriteLine($"{category.Description}");
        var categoryDescription = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(categoryDescription))
        {
            category.Description = categoryDescription;
        }

        try
        {
            // Save updated data to database
            await db.SaveChangesAsync();
            Console.WriteLine("Category Edited");
        }
        catch (DbUpdateException exception)
        {   
            // Print error message and rethrow if needed
            Console.WriteLine(exception.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Delete a category Id
    /// </summary>
    /// <param name="id"></param>
    public static async Task DeleteCategoryAsync(int id)
    {
        using var db =  new StoreContext();
        
        // Find category to delete
        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null)
        {
            Console.WriteLine("Category not found");
            return;
        }
        
        // Remove category
        db.Categories.Remove(category);
        
        try
        {
            // Save deletion changes
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            // Handle DB update exceptions
            Console.WriteLine(exception.Message);
        }
    }
}
