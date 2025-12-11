namespace ProjectDatabases.Models;

// Public class with Customer Properties and a Encrypting/Decrypting - Email
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
    
    // Encrypting-Decrypting email.
    private string? _customerEmail;
    [Required, MaxLength(50)]
    public string? CustomerEmail
    {
        get => _customerEmail == null ? null : EncryptionHelper.Decrypt(_customerEmail);
        set => _customerEmail = string.IsNullOrEmpty(value) ? value : EncryptionHelper.Encrypt(value);
    }

    // Navigation
    public List<Order>? Orders { get; set; } = new();
    
    // Hash Property
    [Required, MaxLength(50)]
    public string SsnHash{ get; set; } = string.Empty;
    
    [Required, MaxLength(50)]
    public string SsnSalt { get; set; } = string.Empty
        ;
}