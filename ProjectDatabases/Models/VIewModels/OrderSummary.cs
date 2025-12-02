using Microsoft.EntityFrameworkCore;

namespace ProjectDatabases.Models.ViewModels;

[Keyless]
public class OrderSummary
{
    public int OrderId { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal OrderTotalPrice { get; set; }
}