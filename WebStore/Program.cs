﻿
using Microsoft.EntityFrameworkCore;
using WebStore.Assignments;
//using WebStore.Entities;
using WebStore.Models;

namespace WebStore
{
    class Program
    {
        static async Task Main(string[] args)
        {

             //TODO: Uncomment this code after generating the entity models

            using var context = new OurCompanyDbContext();


            var Assigments = new LinqQueriesAssignment(context);

            await Assigments.Task01_ListAllCustomers();

            await Assigments.Task02_ListOrdersWithItemCount();

            await Assigments.Task03_ListProductsByDescendingPrice();

            await Assigments.Task04_ListPendingOrdersWithTotalPrice();

            await Assigments.Task05_OrderCountPerCustomer();

            await Assigments.Task06_Top3UsersByOrderValue();

            await Assigments.Task07_RecentOrders();

            await Assigments.Task08_TotalSoldPerProduct();

            await Assigments.Task09_DiscountedOrders();

            await Assigments.Task10_AdvancedQueryExample();
            

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
