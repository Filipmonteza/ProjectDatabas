using Microsoft.EntityFrameworkCore;

namespace ProjectDatabases.Models.ViewModels;

[Keyless]
public class OrderDetailView
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public int TotalRows { get; set; }
    public decimal TotalAmount { get; set; }
}
    
