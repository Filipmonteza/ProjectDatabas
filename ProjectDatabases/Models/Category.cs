using System.ComponentModel.DataAnnotations;

namespace ProjectDatabases.Models;

public class Category
{
    // Prime-Key
    public int CategoryId { get; set; }
    
    // Properties
    [Required, MaxLength(50)]
    public string? CategoryName { get; set; }
    [Required, MaxLength(50)]
    public string Description { get; set; } = string.Empty;
    
    // Navigation
    public List<Product>? Products { get; set; } = new();
    
}