namespace ProjectDatabases.Models;

public class OrderRow
{
    // Prime-Key
    public int OrderRowId { get; set; }
    
    // Foreign-Key
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    
    // Properties
    [Required]
    public int OrderRowQuantity { get; set; }
    [Required]
    public decimal OrderRowUnitPrice { get; set; }
    
    // Navigation
    public Order? Order { get; set; }
    public Product? Product { get; set; }
    
    
}