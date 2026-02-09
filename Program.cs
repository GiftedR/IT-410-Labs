using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

using Week2AdoNetDemo;

string connectionString = "Server=MONTY;Database=IT410Week2Demo;Trusted_Connection=True;TrustServerCertificate=True;";
//ProductRepository repo = new ProductRepository(connectionString);

//// Test GetAll()
//List<Product> all = repo.GetAll();
//Console.WriteLine($"Retrieved {all.Count} products:");
//foreach (var p in all)
//{
//    Console.WriteLine(
//        $"Id={p.ProductId}, Name={p.ProductName}, CategoryId={p.CategoryId}, Price={p.UnitPrice:C}, Active={p.IsActive}");
//}

//// Test GetById()
//Console.WriteLine();
//Console.Write("Enter a ProductId to look up: ");
//string input = Console.ReadLine();
//if (int.TryParse(input, out int id))
//{
//    Product match = repo.GetById(id);
//    if (match != null)
//    {
//        Console.WriteLine("Found:");
//        Console.WriteLine(
//            $"Id={match.ProductId}, Name={match.ProductName}, CategoryId={match.CategoryId}, Price={match.UnitPrice:C}, Active={match.IsActive}");
//    }
//    else
//    {
//        Console.WriteLine("No product found with that ProductId.");
//    }
//}
//else
//{
//    Console.WriteLine("Invalid ProductId.");
//}


//ProductRepository repo = new ProductRepository(connectionString);

//Product p = repo.GetById(1);
//p.ProductName = "Updated Name";
//p.UnitPrice += 1.00m;

//bool updated = repo.UpdateProduct(p);
//Console.WriteLine($"Update success: {updated}");

//bool deleted = repo.DeleteProduct(99);
//Console.WriteLine($"Delete success: {deleted}");

//Week 4

OrderRepository repo = new OrderRepository(connectionString);

// Part A: N+1 version
Stopwatch sw1 = Stopwatch.StartNew();
var ordersA = repo.GetOrdersWithItems_NPlusOne();
sw1.Stop();

Console.WriteLine($"N+1 Version: Orders={ordersA.Count}, Commands={repo.CommandCount}, TimeMs={sw1.ElapsedMilliseconds}");

// Part B: JOIN version
Stopwatch sw2 = Stopwatch.StartNew();
var ordersB = repo.GetOrdersWithItems_Join();
sw2.Stop();

Console.WriteLine($"JOIN Version: Orders={ordersB.Count}, Commands={repo.CommandCount}, TimeMs={sw2.ElapsedMilliseconds}");

// Optional: quick correctness check (spot-check first order)
if (ordersA.Count > 0)
{
    Console.WriteLine();
    Console.WriteLine($"Spot Check - First OrderId={ordersA[0].OrderId}, Items={ordersA[0].Items.Count}");
}