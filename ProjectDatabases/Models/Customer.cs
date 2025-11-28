using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDatabases.Models;

public class Customer
{
    // Prime-Key
    public int CustomerId { get; set; }
    
    [Required, MaxLength(50)]
    public string? CustomerName { get; set; }
    [Required, MaxLength(50)]
    public string? CustomerAddress { get; set; }
    [Required, MaxLength(50)]
    public string? CustomerEmail { get; set; }

    // Navigation
    public List<Order>? Orders { get; set; } = new();
}