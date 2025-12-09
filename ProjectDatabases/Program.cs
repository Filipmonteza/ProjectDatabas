using ProjectDatabases.DataSeeding;
using ProjectDatabases.Services;

// Display the full database path for debugging and verification
Console.WriteLine(Path.Combine(AppContext.BaseDirectory, "Shop.db"));

// Run initial database seeding (creates DB + inserts sample data if needed)
await OrginalSeed.SeedAsync();

// Main Menu with a switch to redirect to sub-menus.
while(true)
{
    Console.WriteLine("\nChoose an option: ");
    Console.WriteLine("1. Customers: ");
    Console.WriteLine("2. Orders: ");
    Console.WriteLine("3. Products: ");
    Console.WriteLine("4. Category: ");
    Console.WriteLine("5. Exit");
    Console.WriteLine(">");
    
    var choice = Console.ReadLine();
    if (choice == "1")
        await CustomerMenu();
    else if (choice == "2")
        await OrderMenu();
    else if (choice == "3")
        await ProductMenu();
    else if (choice == "4")
        await CategoryMenu();
    else if (choice == "5")
        break;
}

// Order-Menu with a switch to redirect to (OrderServices methods).
static async Task OrderMenu()
{
    while (true)
    {
        Console.WriteLine(
            "\nOrder Menu: (1). OrderList | (2). OrderAdd | (3). OrderDetails \n(4). Status-Menu | (5). OrderSummery | (6). OrderDetailView | (7). MainMenu");
        Console.WriteLine(">");
        
        var line = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(line))
        {
            continue;
        }
        
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var cmd = parts[0];

        switch (cmd)
        {
            case "1":
                // Display all orders
                await OrderService.OrderListAsync();
                break;
            case "2":
                // Create a new order
                await OrderService.OrderAddAsync();
                break;
            case"3":
                // Show detailed information for a specific order
                await OrderService.OrderListAsync(); // Call the List Of Customers before Editing.
                if (parts.Length < 2 || !int.TryParse(parts[1], out int detailsId))
                {
                    Console.WriteLine("Usage: User Entry (Details) = 3, (ID Number) -> (Enter) >");
                    break;
                }
                await OrderService.OrderDetailsAsync(detailsId);
                break;
            case "4":
                // Submenu for managing order status
                await OrderService.StatusMenu();
                break;
            case "5":
                // Summary view (aggregated data)
                await OrderService.ListOrderSummery();
                break;
            case"6":
                // Includes order + order items in one combined view
                await OrderService.OrderViewDetail();
                break;
            case "7":
                // Return to main menu
                return;
            default:
                Console.WriteLine("Unkown Command.");
                break;
        }
    }
}
 
// Customer-Menu with a switch redirect to (CustomerService methods)
static async Task CustomerMenu()
    {
        while (true)
        {
            Console.WriteLine("\nCustomer Menu: 1. List | 2. Add | 3. Edit | 4. Delete | \n| 5. CustomerOrderCount | 6. MainMenu ");
            Console.WriteLine(">");

            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0];

            switch (cmd)
            {
                case "1":
                    // Display all customers
                    await CustomerService.CustomerListAsync();
                    break;
                case "2":
                    // Add a new customer
                    await CustomerService.CustomerAddAsync();
                    break;
                case "3":
                    // Edit customer details
                    await CustomerService.CustomerListAsync(); // Call the List Of Customers before Editing.
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int editId))
                    {
                        Console.WriteLine("Usage: User Entry (Edit) = 3, (ID Number) -> (Enter) >");
                        break;
                    }
                    await CustomerService.CustomerEditAsync(editId);
                    break;
                case "4":
                    // Delete a customer
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int deleteId))
                    {
                        Console.WriteLine("Usage: User Entry (Delete) = 4, (ID Number) -> (Enter) >");
                        break;
                    }

                    await CustomerService.CustomerDeleteAsync(deleteId);
                    break;
                case "5":
                    // View number of orders per customer
                    await CustomerService.CustomerOrderCountViews();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Unkown Command.");
                    break;
            }

        }
    }

// Category-Menu Easy switch to redirect to (CategoryService methods)
static async Task CategoryMenu()
    {
        while (true)
        {
            Console.WriteLine("\n== Category ==");
            Console.WriteLine("1. (List) Category.");
            Console.WriteLine("2. (Add) Category.");
            Console.WriteLine("3. (Edit) Category.");
            Console.WriteLine("4. (Delete) Category.");
            Console.WriteLine("5. Main Menu.");
            Console.WriteLine(">");

            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0];

            switch (cmd)
            {
                case "1":
                    await CategoryService.ListCategoryAsync();
                    break;
                 case "2":
                    await CategoryService.AddCategoryAsync();
                    break;
                case "3":
                    await CategoryService.ListCategoryAsync(); // Call the List Of Customers before Editing.
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var editId))
                    {
                        Console.WriteLine("Usage: User Entry (Edit) = 3, (ID Number) -> (Enter) >");
                        break;
                    }
                    await CategoryService.EditCategoryAsync(editId);
                    break;
                case "4":
                    await CategoryService.ListCategoryAsync();
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var deleteId))
                    {
                        Console.WriteLine("Usage: User Entry (Delete) = 4, (ID Number) -> (Enter) >");
                        break;
                    }
                    await CategoryService.DeleteCategoryAsync(deleteId);
                     break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Unkown Command.");
                    break;
            }

        }
    }

// Product-Menu with a switch to redirect to (ProductService methods)
static async Task ProductMenu()
    {
        while (true)
        {
            Console.WriteLine("\n== Products ==");
            Console.WriteLine("1. (List) Product.");
            Console.WriteLine("2. (Add) Product.");
            Console.WriteLine("3. (Edit) Product.");
            Console.WriteLine("4. (Delete) Product.");
            Console.WriteLine("5. (TotalSales) Product.");
            Console.WriteLine("6. Main Menu.");
            Console.WriteLine(">");

            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0];

            switch (cmd)
            {
                case "1":
                    await ProductService.ListProductAsync();
                    break;
                 case "2":
                    // Display categories first (for selection when adding a product)
                    await CategoryService.ListCategoryAsync(); 
                    await ProductService.AddProductAsync();
                    break;
                case "3":
                    await ProductService.ListProductAsync(); // Call the List Of Customers before Editing.
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var editId))
                    {
                        Console.WriteLine("Usage: User Entry (Edit) = 3, (ID Number) -> (Enter) >");
                        break;
                    }
                    await ProductService.EditProductAsync(editId);
                    break;
                case "4":
                    await ProductService.ListProductAsync();
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var deleteId))
                    {
                        Console.WriteLine("Usage: User Entry (Delete) = 4, (ID Number) -> (Enter) >");
                        break;
                    }
                    await ProductService.DeleteProductAsync(deleteId);
                     break;
                case "5":
                    // View aggregated sales per product
                    await  ProductService.ProductSalesViews();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Unkown Command.");
                    break;
            }
        }
    }

    






