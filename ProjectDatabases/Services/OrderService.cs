using Microsoft.EntityFrameworkCore;

namespace ProjectDatabases.Services;

public class OrderService
{
    public static async Task ListOrderSummery()
    {
        using var db = new StoreContext();
        var summaries = await db.OrderSummaries.OrderByDescending(o=>o.OrderDate).ToListAsync();
       
        Console.WriteLine(" OrderId | OrderDate | TotalAmount | CustomerEmail");
        foreach (var summary in summaries)
        {
            Console.WriteLine($"{summary.OrderId} | {summary.OrderDate} | {summary.OrderTotalPrice} | {summary.CustomerEmail}");
            
        }
    }
    
    public static async Task OrderListAsync()
    {
        using var db = new StoreContext();
        
        var orders = await db.Orders
            .AsNoTracking()
            .OrderBy(c=> c.OrderId).Include(c=> c.Customer)
            .ToListAsync();
        
        Console.WriteLine("\n ==== Orders ====");
        Console.WriteLine("OrderId | Customer-Name | OrderDate| TotalAmount | Status ");

        foreach (var order in orders)
        {
            Console.WriteLine($"{order.OrderId} | {order.Customer?.CustomerName} | {order.OrderDate} | {order.OrderTotalPrice} | {order.OrderStatus}");
        }
    }
    
    public static async Task OrderDetailsAsync(int detailsId)
    {
        using var db = new StoreContext();

        var orderdetails = await db.Orders
            .AsNoTracking()
            .OrderBy(x => x.OrderId)
            .Include(o => o.OrderRows)
            .ThenInclude(x => x.Product)
            .ToListAsync();
        Console.WriteLine("Order Details:");
        Console.WriteLine("OrderID | ProductName | Quantity | Price per unit | Row Total");
        foreach (var order in orderdetails.Where(o => o.OrderId == detailsId))
        {
            foreach (var orderRow in order.OrderRows!) 
            {
                var rowTotal = orderRow.OrderRowQuantity * orderRow.OrderRowUnitPrice; // 1 Rad
                Console.WriteLine($"{order.OrderId} | {orderRow.Product?.ProductName} | {orderRow.OrderRowQuantity} | {orderRow.OrderRowUnitPrice} | {rowTotal}");
            }
            
            var orderTotal = order.OrderRows.Sum(o => o.OrderRowQuantity * o.OrderRowUnitPrice);
            Console.WriteLine($"Total Amount: {orderTotal}");
        }
    }
    
    public static async Task OrderAddAsync()
    {
        await CustomerService.CustomerListAsync();
        using var db = new StoreContext();
        Console.WriteLine("Enter Customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out var customerId))
        {
            Console.WriteLine("Invalid Customer ID.");
            return;
        }
        
        var customer = await db.Customers.FindAsync(customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.WriteLine("Product available: ");
        var products = await db.Products.AsNoTracking()
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        Console.WriteLine("ProductId | ProductName | ProductPrice ");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice} ");
        }
        
        var newOrder = new Order
        {   
            OrderDate = DateTime.Now,
            CustomerId = customer.CustomerId,
            OrderStatus = "Pending",
            OrderRows = new List<OrderRow>()
        };
        
        while (true)
        {
            Console.WriteLine("Enter Product ID, Or 'done' to exit:");
            var input = Console.ReadLine();
            if (input?.ToLower() == "done") break;

            if (!int.TryParse(input, out var productId))
            {
                Console.WriteLine("Invalid Product ID.");
                continue;
            }

            var product = await db.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                continue;
            }

            Console.WriteLine("Enter Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int orderRowQuantity))
            {
                Console.WriteLine("Invalid Quantity.");
                continue;
            }
            
            var orderRow = new OrderRow
            {
                ProductId = productId,
                OrderRowQuantity = orderRowQuantity,
                OrderRowUnitPrice = product.ProductPrice
            };
            newOrder.OrderRows.Add(orderRow);
        }
        
        // Saving and adding
        db.Orders.Add(newOrder);
        await db.SaveChangesAsync();
        
        // Printing
        Console.WriteLine($" Order ID: {newOrder.OrderId} created for {customer.CustomerName} with total {newOrder.OrderTotalPrice} Status {newOrder.OrderStatus}");
    }
    
    // New Switch for StatusMenu
    public static async Task StatusMenu()
    {
        while (true)
        {
            Console.WriteLine("\nStatus Menu: 1. Order-Status | 2. Customer-Status | 3. Page-Order | 4. Back ");
            Console.WriteLine(">");
        
            var line =  Console.ReadLine()?.Trim() ?? string.Empty;
            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            
            var parts =  line.Split(' ', StringSplitOptions.RemoveEmptyEntries );
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
            {
                case "1":
                    await OrderListAsync();
                    if (parts.Length < 2 ) { Console.WriteLine("Usage: User Entry (Filter Status) = (1 -> <Status> -> Enter))"); break; } ;
                    await OrderByStatusAsync(parts[1]);
                    break;
                case"2":
                    await CustomerService.CustomerListAsync();
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int customerId))
                    {
                        Console.WriteLine("Usage: User Entry (specify Customer) = 2, (ID Number) -> (Enter) >");
                        break;
                    }
                    await OrdersByCustomer(customerId);
                    break;
                case"3":
                    await OrdersPage(1,10);
                    break;
                case"4":
                    return;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
       
    }
    public static async Task OrderByStatusAsync(string status)
    {   
        using var db = new StoreContext();
        var orders = await db.Orders
            .Include(o=> o.Customer)
            .Where(o => o.OrderStatus
                .ToLower() == status.ToLower())
            .OrderBy(o=> o.OrderDate)
            .ToListAsync();
        
        Console.WriteLine($"\nOrders with status: {status}");
        foreach (var order in orders)
        {
            Console.WriteLine($"{order.OrderId} | {order.Customer?.CustomerName} | {order.OrderTotalPrice} | {order.OrderDate:d} |{order.OrderStatus}");
        }
    }
    
    public static async Task OrdersByCustomer(int customerId)
    {
        using var db = new StoreContext();
        var orders = await db.Orders
            .Include(o=> o.Customer)
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o=> o.OrderDate)
            .ToListAsync();
        
        Console.WriteLine($"\nOrders for CustomerId: {customerId}");
        foreach (var order in orders)
        {
            Console.WriteLine($"{order.OrderId} | {order.Customer?.CustomerName} | {order.OrderTotalPrice} | {order.OrderDate:d} |{order.OrderStatus}");
        }
    }
    
     public static async Task OrdersPage(int page, int pageSize)
    {
        using var db = new StoreContext();
        int currentPage = Math.Max(page, 1);
        
        while (true)
        {
            
            var query = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderRows)
                .ThenInclude(or => or.Product)
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking();
    
            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
    
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;
    
            var pageItems = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    
            Console.WriteLine($"\nPage {currentPage}/{totalPages} (pageSize={pageSize}, total={totalCount})");
            Console.WriteLine("CustomerName | CustomerID | OrderDate | TotalAmount | OrderStatus");
            foreach (var order in pageItems)
            {
                Console.WriteLine($"{order.Customer?.CustomerName} | {order.Customer?.CustomerId} | {order.OrderDate} | {order.OrderTotalPrice} | {order.OrderStatus}");
            }
    
            Console.WriteLine("\nCommands: n = next, p = previous, q = quit");
            Console.Write("Enter command: ");
            var cmd = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
    
            if (cmd == "q") break;
            if (cmd == "n")
            {
                if (currentPage < totalPages) currentPage++;
                else Console.WriteLine("Already on last page.");
                continue;
            }
            if (cmd == "p")
            {
                if (currentPage > 1) currentPage--;
                else Console.WriteLine("Already on first page.");
                continue;
            }
    
            Console.WriteLine("Unknown command.");
        }
    }

    public static async Task OrderViewDetail()
    {
        using var db = new StoreContext();
        var orderDetail = await db.OrderDetailViews
            .OrderByDescending(o=>o.OrderDate)
            .ToListAsync();
       
        Console.WriteLine("OrderDetailView");
        Console.WriteLine(" OrderId | CustomerName | OrderTotalPrice | OrderDate");
        foreach (var orderDetailView in orderDetail)
        {
            Console.WriteLine($"{orderDetailView.OrderId} | {orderDetailView.CustomerName} | {orderDetailView.TotalAmount} | {orderDetailView.OrderDate}");
            
        }
    }
}
