namespace ProjectDatabases.Models;

public class Order
{
    //Prime-Key
    public int OrderId { get; set; }
    
    // Foreign-Key
    public int CustomerId { get; set; }
    
    // Properties
    [Required, MaxLength(50)]
    public string? OrderStatus { get; set; }
    [Required]
    public decimal OrderTotalPrice { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    // Navigation
    public Customer? Customer { get; set; }
    public List<OrderRow>? OrderRows { get; set; } = new();



}