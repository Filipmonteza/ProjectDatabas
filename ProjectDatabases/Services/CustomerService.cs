namespace ProjectDatabases.Services;

public class CustomerService
{
    // List all customers in the database and print them in a structured format
    public static async Task CustomerListAsync()
    {
        using var db = new StoreContext();
        
        // Retrieve customers without tracking for better performance
        var customers = await db.Customers
            .AsNoTracking()
            .OrderBy(c=> c.CustomerId)
            .ToListAsync();
        
        Console.WriteLine("\n ==== Customers ====");
        Console.WriteLine("ID | Name | Address | Email");
        
        // Display each customer
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId}, {customer.CustomerName}, {customer.CustomerAddress}, {customer.CustomerEmail}");
        }
    }
    
    /// <summary>
    /// Adds a new customer to the database
    /// </summary>
    public static async Task CustomerAddAsync()
    {
        Console.WriteLine("Customer Name: ");
        var customerName = Console.ReadLine()?.Trim() ?? string.Empty;

        // Validate CustomerName
        if (string.IsNullOrEmpty(customerName) || customerName.Length > 50)
        {
            Console.WriteLine("Customer Name is required (Max 50). ");
        }
        
        Console.WriteLine("Customer Email: ");
        var customeremail = Console.ReadLine()?.Trim() ?? string.Empty;
        
        // Validate CustomerEmail
        if (string.IsNullOrEmpty(customeremail) || customeremail.Length > 50)
        {
            Console.WriteLine("Customer Email is required (Max 50). ");
        }
        
        Console.WriteLine("Customer Address (Optional): ");
        var customerAddress = Console.ReadLine()?.Trim() ?? string.Empty;

        using var db = new StoreContext();
        
        // Adds a new customer entity
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
    
    /// <summary>
    /// Edits an existing Customers
    /// </summary>
    /// <param name="customerId"></param>
    public static async Task CustomerEditAsync(int customerId)
    {
        using var db = new StoreContext();
        
        // Retrieve the customer that should be edited
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
       
        // Edit CustomerName
        Console.WriteLine($"Edit: {customer.CustomerName}");
        var customername = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(customername))
        {
            customer.CustomerName = customername;
        }

        // Edit CustomerEmail
        Console.WriteLine($"Edit: {customer.CustomerEmail}");
        var customeremail = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(customeremail))
        {
            customer.CustomerEmail = customeremail;
        }
        
        // Edit CustomerAddress
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
    
    /// <summary>
    /// Deletes a Customer from the database
    /// </summary>
    /// <param name="deleteId"></param>
    public static async Task CustomerDeleteAsync(int deleteId)
    {
        using var db = new StoreContext();
        
        // Try to find matching Customer
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == deleteId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
        
        // Remove Customer context
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
    
    /// <summary>
    /// Shows customers along with number of associated orders from a view
    /// </summary>
    public static async Task CustomerOrderCountViews()
    {
        using var db = new StoreContext();
        
        // fetch rows from CustomerOrderCountView
        var cocV = await db.CustomerOrderCountViews
            .OrderByDescending(c=> c.CustomerId)
            .ToListAsync();
        
        Console.WriteLine("CustomerId | CustomerName | CustomerEmail | NumberOfOrders");
        
        // Display results
        foreach (var customer in cocV)
        {
            Console.WriteLine($"{customer.CustomerId} | {customer.CustomerName} | {customer.CustomerEmail} | {customer.NumberOfOrders}");
        }
    }
}
