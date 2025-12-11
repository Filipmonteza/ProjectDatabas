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
        Console.WriteLine("ID | Name | Address | Email | Orders | SSN Hash | SSN Salt");
        
        // Display each customer
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId}, {customer.CustomerName}, {customer.CustomerAddress}, {customer.CustomerEmail}, {customer.SsnHash}, {customer.SsnSalt}");
        }
    }
    
    /// <summary>
    /// Adds a new customer to the database
    /// Rollback implements if an error occurs
    /// Social Security Number (SSN) is hashed with salt for security
    /// </summary>
    public static async Task CustomerAddAsync()
    {
        using var db = new StoreContext();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
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
            
            Console.WriteLine("Please enter the Social Security Number (SSN) for the customer:");
            var ssnInput = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(ssnInput))
            {
                Console.WriteLine("SSN is required.");
                return;
            }
            
            var salt = Hashinghelper.Generatesalt();
            var ssnHash = Hashinghelper.HasWithSalt(ssnInput, salt);
            
        
            Console.WriteLine("Customer Address (Optional): ");
            var customerAddress = Console.ReadLine()?.Trim() ?? string.Empty;
        
            // Adds a new customer entity
            db.Customers.Add(new Customer {CustomerName = customerName, CustomerAddress = customerAddress, CustomerEmail = customeremail, SsnHash = ssnHash, SsnSalt = salt});
            
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
            Console.WriteLine("Customer Added Successfully");
            
        }
        catch (DbUpdateException exception)
        {
            await transaction.RollbackAsync();
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
    
    /// <summary>
    /// Retrieves and displays all customers together with their orders.
    /// </summary>
    /// <remarks>
    /// This method performs a read-only query using <c>AsNoTracking()</c> to improve performance
    /// when no entity updates are needed. It loads customers ordered by CustomerId and includes
    /// their associated orders via <c>Include(x => x.Orders)</c>.
    ///
    /// A stopwatch (System.Diagnostics.Stopwatch) is used to measure query execution time,
    /// useful when verifying performance or comparing tracking vs. no-tracking queries.
    ///
    /// The commented section at the top contains optional seed logic that can be enabled to
    /// generate a large test dataset (100 customers with 2 orders each). This is helpful for
    /// pagination, performance testing, and verifying query behavior with high row counts.
    /// </remarks>
    public static async Task CustomerListAndOrdersAsync()
    {
        using var db = new StoreContext();
        
        // If you want to Seed 100 Customers with 2 Orders each, uncomment below
        // for (int i = 1; i <= 100; i++) // Create 5 orders
        // {
        //     var customers = new Customer
        //     {
        //         CustomerName = $"Test Customer {i}",
        //         CustomerAddress = $"Street {i}",
        //         CustomerEmail = $"customer1{i}@mail.com",
        //         Orders = new List<Order>()
        //     };
        //
        //     // Create 2 orders per customer
        //     for (int j = 1; j <= 2; j++) 
        //     {
        //         customers.Orders.Add(new Order
        //         {
        //             OrderDate = DateTime.Now.AddDays(-j),
        //             OrderTotalPrice = 100 + j * 10,
        //             OrderStatus = "Pending"
        //         });
        //     }
        //
        //     db.Customers.Add(customers);
        // }
        //
        // await db.SaveChangesAsync();
        
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var customer = await db.Customers
            .AsNoTracking()
            .OrderBy(c => c.CustomerId)
            .Include(x => x.Orders)
            .ToListAsync();
        
        Console.WriteLine("\n ==== Customers and their Orders ====");
        Console.WriteLine("ID | Name | Address | Email | Orders");
        
        sw.Stop();
        Console.WriteLine($"Time elapsed: {sw.ElapsedMilliseconds} ms");
        
        foreach(var cust in customer)
        {
            if(cust.Orders != null)
            {
                Console.WriteLine($"{cust.CustomerId}, {cust.CustomerName}, {cust.CustomerAddress}, {cust.CustomerEmail}, {cust.Orders?.Count}");
                
            }
        }
    }
}

