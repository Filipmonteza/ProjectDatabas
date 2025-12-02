using ProjectDatabases.DataSeeding;
using ProjectDatabases.Services;

Console.WriteLine(Path.Combine(AppContext.BaseDirectory, "Shop.db"));

// Extracting seeding - from (OriginalSeed).
await OrginalSeed.SeedAsync();

// Main Menu
while(true)
{
    Console.WriteLine("\nChoose an option: ");
    Console.WriteLine("1. Customers: ");
    Console.WriteLine("2. Orders: ");
    Console.WriteLine("3. Products: ");
    Console.WriteLine("4. Category: ");
    Console.WriteLine("5. Exit: ");
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
    else
    {
        Console.WriteLine("Invalid choice");
    }
}


static async Task OrderMenu()
{
    while (true)
    {
        Console.WriteLine(
            "\nOrder Menu: (1). OrderList | (2). OrderAdd | (3). OrderDetails \n(4). Status-Menu | (5). OrderSummery | (6). MainMenu");
        Console.WriteLine(">");
        var line = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(line))
        {
            continue;
        }

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var cmd = parts[0].ToLowerInvariant();

        switch (cmd)
        {
            case "1":
                await OrderService.OrderListAsync();
                break;
            case "2":
                await OrderService.OrderAddAsync();
                break;
            case"3":
                await OrderService.OrderListAsync(); // Call the List Of Customers before Editing.
                if (parts.Length < 2 || !int.TryParse(parts[1], out int detailsId))
                {
                    Console.WriteLine("Usage: User Entry (Details) = 3, (ID Number) -> (Enter) >");
                    break;
                }
                await OrderService.OrderDetailsAsync(detailsId);
                break;
            case "4":
                await OrderService.StatusMenu();
                break;
            case "5":
                await OrderService.ListOrderSummery();
                break;
            case "6":
                return;
            default:
                Console.WriteLine("Unkown Command.");
                break;
        }
    }
}
 
static async Task CustomerMenu()
    {
        while (true)
        {
            Console.WriteLine("\nCustomer Menu: 1. List | 2. Add | 3. Edit | 4. Delete | 5. MainMenu ");
            Console.WriteLine(">");

            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
            {
                case "1":
                    await CustomerService.CustomerListAsync();
                    break;
                case "2":
                    await CustomerService.CustomerAddAsync();
                    break;
                case "3":
                    await CustomerService.CustomerListAsync(); // Call the List Of Customers before Editing.
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int editId))
                    {
                        Console.WriteLine("Usage: User Entry (Edit) = 3, (ID Number) -> (Enter) >");
                        break;
                    }

                    await CustomerService.CustomerEditAsync(editId);
                    break;
                case "4":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int deleteId))
                    {
                        Console.WriteLine("Usage: User Entry (Delete) = 4, (ID Number) -> (Enter) >");
                        break;
                    }

                    await CustomerService.CustomerDeleteAsync(deleteId);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Unkown Command.");
                    break;
            }

        }
    }

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

            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();

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
static async Task ProductMenu()
    {
        
        while (true)
        {
            Console.WriteLine("\n== Products ==");
            Console.WriteLine("1. (List) Product.");
            Console.WriteLine("2. (Add) Product.");
            Console.WriteLine("3. (Edit) Product.");
            Console.WriteLine("4. (Delete) Product.");
            Console.WriteLine("5. Main Menu.");
            Console.WriteLine(">");

            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
            {
                case "1":
                    await ProductService.ListProductAsync();
                    break;
                 case "2":
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
                    return;
                default:
                    Console.WriteLine("Unkown Command.");
                    break;
            }

        }
    }




