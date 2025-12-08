namespace ProjectDatabases.Models;

public class Product
{
    // Prime-Key
    public int ProductId { get; set; }
    
    // Foreign-Key
    public int CategoryId { get; set; }
    
    // Properties
    [Required, MaxLength (50)]
    public string? ProductName { get; set; }
    [Required]
    public decimal ProductPrice { get; set; }
    
    // Navigation
    public Category? Category { get; set; }
    
}