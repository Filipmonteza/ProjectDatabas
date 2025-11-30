using ProjectDatabases.DataSeeding;

Console.WriteLine(Path.Combine(AppContext.BaseDirectory, "Shop.db"));

// Extracting seeding - from (DataSeeding).
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
        await ProductMenu;
    else if (choice == "4")
        await CategoryMenu;
    else if (choice == "5")
        break;
    else
    {
        Console.WriteLine("Invalid choice");
    }

}