using Microsoft.EntityFrameworkCore;


namespace ProjectDatabases.Models.ViewModels;
/// <summary>
/// View Model for ProductSalesView
/// </summary>
[Keyless]
public class ProductSalesView
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
}