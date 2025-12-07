using Microsoft.EntityFrameworkCore;

namespace ProjectDatabases.Services;

public class CustomerService
{
    public static async Task CustomerListAsync()
    {
        using var db = new StoreContext();
        
        var customers = await db.Customers
            .AsNoTracking()
            .OrderBy(c=> c.CustomerId)
            .ToListAsync();
        
        Console.WriteLine("\n ==== Customers ====");
        Console.WriteLine("ID | Name | Address | Email");

        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId}, {customer.CustomerName}, {customer.CustomerAddress}, {customer.CustomerEmail}");
        }
    }
    
    public static async Task CustomerAddAsync()
    {
        
        Console.WriteLine("Customer Name: ");
        var customerName = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(customerName) || customerName.Length > 50)
        {
            Console.WriteLine("Customer Name is required (Max 50). ");
        }
        
        Console.WriteLine("Customer Email: ");
        var customeremail = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(customeremail) || customeremail.Length > 50)
        {
            Console.WriteLine("Customer Email is required (Max 50). ");
        }
        
        Console.WriteLine("Customer Address (Optional): ");
        var customerAddress = Console.ReadLine()?.Trim() ?? string.Empty;

        using var db = new StoreContext();
        db.Customers.Add(new Customer {CustomerName = customerName, CustomerAddress = customerAddress, CustomerEmail = customeremail});
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer Added Successfully");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine("Db Error (Maby duplicate)" + exception.Message);
        }
        
    }
    
    public static async Task CustomerEditAsync(int customerId)
    {
        
        using var db = new StoreContext();
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
       
        Console.WriteLine($"Edit: {customer.CustomerName}");
        var customername = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(customername))
        {
            customer.CustomerName = customername;
        }

        Console.WriteLine($"Edit: {customer.CustomerEmail}");
        var customeremail = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(customeremail))
        {
            customer.CustomerEmail = customeremail;
        }
        
        Console.WriteLine($"Edit: {customer.CustomerAddress}");
        var customerAddress = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(customerAddress))
        {
            customer.CustomerAddress = customerAddress;
        }

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer Edit Successfully");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }
    }
    
    
    public static async Task CustomerDeleteAsync(int deleteId)
    {
        using var db = new StoreContext();
        
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == deleteId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
        db.Customers.Remove(customer);
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer deleted successfully");
        }
        catch (DbUpdateException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public static async Task CustomerOrderCountViews()
    {
        using var db = new StoreContext();
        var cocV = await db.CustomerOrderCountViews
            .OrderByDescending(c=> c.CustomerId)
            .ToListAsync();
        
        Console.WriteLine("CustomerId | CustomerName | CustomerEmail | NumberOfOrders");
        foreach (var customer in cocV)
        {
            Console.WriteLine($"{customer.CustomerId} | {customer.CustomerName} | {customer.CustomerEmail} | {customer.NumberOfOrders}");
        }
    }
}
