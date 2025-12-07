using System.ComponentModel.DataAnnotations;

namespace ProjectDatabases.Models;


public class Customer
{
    // Prime-Key
    public int CustomerId { get; set; }
    
    // Properties
    [Required, MaxLength(50)]
    public string? CustomerName { get; set; }
    [Required, MaxLength(50)]
    public string? CustomerAddress { get; set; }
    [Required, MaxLength(50)]
    
    // Encrypting-Decrypting
    private string? _customerEmail;
    [Required, MaxLength(50)]
    public string? CustomerEmail
    {
        get => _customerEmail == null ? null : EncryptionHelper.Decrypt(_customerEmail);
        set => _customerEmail = string.IsNullOrEmpty(value) ? value : EncryptionHelper.Encrypt(value);
    }

    // Navigation
    public List<Order>? Orders { get; set; } = new();
}