using System.Xml;
using Microsoft.EntityFrameworkCore;
//using WebStore.Entities;
using WebStore.Models;

namespace WebStore.Assignments
{
    /// Additional tutorial materials https://dotnettutorials.net/lesson/linq-to-entities-in-entity-framework-core/

    /// <summary>
    /// This class demonstrates various LINQ query tasks 
    /// to practice querying an EF Core database.
    /// 
    /// ASSIGNMENT INSTRUCTIONS:
    ///   1. For each method labeled "TODO", write the necessary
    ///      LINQ query to return or display the required data.
    ///      
    ///   2. Print meaningful output to the console (or return
    ///      collections, as needed).
    ///      
    ///   3. Test each method by calling it from your Program.cs
    ///      or test harness.
    /// </summary>
    public class LinqQueriesAssignment
    {

        // TODO: Uncomment this code after generating the entity models

        private readonly OurCompanyDbContext _dbContext;

        public LinqQueriesAssignment(OurCompanyDbContext context)
        {
            _dbContext = context;
        }


        /// <summary>
        /// 1. List all customers in the database:
        ///    - Print each customer's full name (First + Last) and Email.
        /// </summary>
        public async Task Task01_ListAllCustomers()
        {
            // TODO: Write a LINQ query that fetches all customers
            //       and prints their names + emails to the console.
            // HINT: context.Users
            
            var users = await _dbContext.Users
               // .AsNoTracking() // optional for read-only
               .ToListAsync();

            Console.WriteLine("=== TASK 01: List All Users ===");

            foreach (var c in users)
            {
                Console.WriteLine($"{c.UFirstname} {c.ULastname} - {c.UEmail}");
            }

            
        }

        /// <summary>
        /// 2. Fetch all orders along with:
        ///    - Customer Name
        ///    - Order ID
        ///    - Order Status
        ///    - Number of items in each order (the sum of OrderItems.Quantity)
        /// </summary>
        public async Task Task02_ListOrdersWithItemCount()
        {
            // TODO: Write a query to return all orders,
            //       along with the associated customer name, order status,
            //       and the total quantity of items in that order.

            // HINT: Use Include/ThenInclude or projection with .Select(...).
            //       Summing the quantities: order.OrderItems.Sum(oi => oi.Quantity).

            
            Console.WriteLine(" ");
            Console.WriteLine("=== TASK 02: List Orders With Item Count ===");

            var orders = await _dbContext.Orders
                .Select(order => new
                {
                    OrderId = order.OrderId,
                    CustomerName = order.UIdNavigation.UFirstname,
                    Status = order.OrdStatus,
                    TotalQuantity = order.OrderedProducts.Count()
                })
                .ToListAsync();

                foreach (var o in orders)
                {
                    Console.WriteLine($"Order ID: {o.OrderId}, Customer: {o.CustomerName}, Status: {o.Status}, Total Items: {o.TotalQuantity}");
                }

        }

        /// <summary>
        /// 3. List all products (ProductName, Price),
        ///    sorted by price descending (highest first).
        /// </summary>
        public async Task Task03_ListProductsByDescendingPrice()
        {
            // TODO: Write a query to fetch all products and sort them
            //       by descending price.
            // HINT: context.Products.OrderByDescending(p => p.Price)
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 03: List Products By Descending Price ===");

            var products = _dbContext.Products
                .OrderByDescending(p => p);

            foreach (var p in products)
            {
                Console.WriteLine($"{p.ProdDesc} - {p.ProdPrice}");
            }  
        }

        /// <summary>
        /// 4. Find all "Pending" orders (order status = "Pending")
        ///    and display:
        ///      - Customer Name
        ///      - Order ID
        ///      - Order Date
        ///      - Total price (sum of unit_price * quantity - discount) for each order
        /// </summary>
        public async Task Task04_ListPendingOrdersWithTotalPrice()
        {
            // TODO: Write a query to fetch only PENDING orders,
            //       and calculate their total price.
            // HINT: The total can be computed from each OrderItem:
            //       (oi.UnitPrice * oi.Quantity) - oi.Discount
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 04: List Pending Orders With Total Price ===");

            var orders = await _dbContext.Orders
                .Where(order => order.OrdStatus == "Pending")
                .Select(order => new
                {
                    OrderId = order.OrderId,
                    Status = order.OrdStatus,
                    TotalPrice = order.OrdTotal
                })
                .ToListAsync();
                foreach (var o in orders)
                {
                    Console.WriteLine($"Order {o.OrderId}: {o.Status} - Price: {o.TotalPrice}");
                }  
        }

        /// <summary>
        /// 5. List the total number of orders each customer has placed.
        ///    Output should show:
        ///      - Customer Full Name
        ///      - Number of Orders
        /// </summary>
        public async Task Task05_OrderCountPerCustomer()
        {
            // TODO: Write a query that groups by Customer,
            //       counting the number of orders each has.

            // HINT: 
            //  1) Join Orders and Users, or
            //  2) Use the navigation (context.Orders or context.Users),
            //     then group by customer ID or by the customer entity.
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 05: Order Count Per Customer ===");

            var OrderCounts = await _dbContext.Orders
                .GroupBy(o => o.UIdNavigation)  
                .Select(u => new 
                {
                    UserFName = u.Key.UFirstname,  
                    UserLName = u.Key.ULastname,  
                    NumberOfOrders = u.Count()  
                })
                .ToListAsync();

            foreach (var c in OrderCounts)
            {
                Console.WriteLine($"Customer: {c.UserFName} {c.UserLName}, Number of Orders: {c.NumberOfOrders}");
            }
        }

        /// <summary>
        /// 6. Show the top 3 customers who have placed the highest total order value overall.
        ///    - For each customer, calculate SUM of (OrderItems * Price).
        ///      Then pick the top 3.
        /// </summary>
        public async Task Task06_Top3UsersByOrderValue()
        {
            // TODO: Calculate each customer's total order value 
            //       using their Orders -> OrderItems -> (UnitPrice * Quantity - Discount).
            //       Sort descending and take top 3.

            // HINT: You can do this in a single query or multiple steps.
            //       One approach:
            //         1) Summarize each Order's total
            //         2) Summarize for each Customer
            //         3) Order by descending total
            //         4) Take(3)
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 06: Top 3 Users By Order Value ===");

            var topCustomers = await _dbContext.Orders
                .Select(o => new
                {
                    User = o.UIdNavigation,  
                    TotalOrderValue = o.OrdTotal
                })
                .GroupBy(o => o.User)  
                .Select(g => new
                {
                    User = g.Key,
                    TotalSpent = g.Sum(x => x.TotalOrderValue)  
                })
                .OrderByDescending(x => x.TotalSpent)  
                .Take(3)  
                .ToListAsync();

            foreach (var c in topCustomers)
            {  
                Console.WriteLine($"Customer: {c.User.UFirstname}, Total Spent: {c.TotalSpent}");
            }

        }

        /// <summary>
        /// 7. Show all orders placed in the last 30 days (relative to now).
        ///    - Display order ID, date, and customer name.
        /// </summary>
        public async Task Task07_RecentOrders()
        {
            // TODO: Filter orders to only those with OrderDate >= (DateTime.Now - 30 days).
            //       Output ID, date, and the customer's name.
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 07: Recent Orders ===");


            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var recentOrders = await _dbContext.Orders
                .Where(o => o.OrdDate >= DateOnly.FromDateTime(thirtyDaysAgo))  
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    OrdDate = o.OrdDate, 
                    User = o.UIdNavigation.UFirstname 
                })
                .ToListAsync();

            foreach (var order in recentOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Date: {order.OrdDate}, Customer: {order.User}");
            }

        }

        /// <summary>
        /// 8. For each product, display how many total items have been sold
        ///    across all orders.
        ///    - Product name, total sold quantity.
        ///    - Sort by total sold descending.
        /// </summary>
        public async Task Task08_TotalSoldPerProduct()
        {
            // TODO: Group or join OrdersItems by Product.
            //       Summation of quantity.
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 08: Total Sold Per Product ===");

            var productSales = await _dbContext.OrderedProducts
                .GroupBy(op => op.ProdId)  
                .Select(group => new
                {
                    ProductName = group.FirstOrDefault().Prod.ProdDesc,  
                    TotalSold = group.Count()  
                })
                .OrderByDescending(x => x.TotalSold)  
                .ToListAsync();

            foreach (var sale in productSales)
            {
                Console.WriteLine($"Product: {sale.ProductName}, Total Sold: {sale.TotalSold}");
            }

        }

        /// <summary>
        /// 9. List any orders that have at least one OrderItem with a Discount > 0.
        ///    - Show Order ID, Customer name, and which products were discounted.
        /// </summary>
        public async Task Task09_DiscountedOrders()
        {
            // TODO: Identify orders with any OrderItem having (Discount > 0).
            //       Display order details, plus the discounted products.
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 09: Discounted Orders ===");
            
            Console.WriteLine("We don't have discounts!!!");

            

        }

        /// <summary>
        /// 10. (Open-ended) Combine multiple joins or navigation properties
        ///     to retrieve a more complex set of data. For example:
        ///     - All orders that contain products in a certain category
        ///       (e.g., "Electronics"), including the store where each product
        ///       is stocked most. (Requires `Stocks`, `Store`, `ProductCategory`, etc.)
        ///     - Or any custom scenario that spans multiple tables.
        /// </summary>
        public async Task Task10_AdvancedQueryExample()
        {
            // TODO: Design your own complex query that demonstrates
            //       multiple joins or navigation paths. For example:
            //       - Orders that contain any product from "Electronics" category.
            //       - Then, find which store has the highest stock of that product.
            //       - Print results.

            // Here's an outline you could explore:
            // 1) Filter products by category name "Electronics"
            // 2) Find any orders that contain these products
            // 3) For each of those products, find the store with the max stock
            //    (requires .MaxBy(...) in .NET 6+ or custom code in older versions)
            // 4) Print a combined result

            // (Implementation is left as an exercise.)
            Console.WriteLine(" ");
            Console.WriteLine("=== Task 10: Advanced Query Example ===");


           
        
            var ordersWithMobilePhones = await _dbContext.Orders
                .Where(o => o.OrderedProducts
                    .Any(op => op.Prod.ProdCategory == "Mobile phones"))  
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    CustomerName = o.UIdNavigation.UFirstname,  
                    Products = o.OrderedProducts
                        .Where(op => op.Prod.ProdCategory == "Mobile phones") 
                        .Select(op => new 
                        {
                            ProductName = op.Prod.ProdDesc,
                            ProductId = op.Prod.ProdId,
                        })
                        .ToList()
                })
                .ToListAsync();

           
            foreach (var order in ordersWithMobilePhones)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerName}");
                
                foreach (var product in order.Products)
                {
                    
                    Console.WriteLine($"  Product: {product.ProductName}, Quantity in Order: 1");
                }
                Console.WriteLine(); 
            }



        }
        
    }
}
