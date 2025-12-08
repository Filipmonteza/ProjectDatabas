namespace ProjectDatabases.Models.ViewModels;

// View Model for CustomerOrderCountView
[Keyless]
public class CustomerOrderCountView
{
    public int CustomerId { get; set; }
    public string  CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public int  NumberOfOrders { get; set; }
}