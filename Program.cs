internal class Program
{
	const string connection_string = $"Server=localhost;Database=IT410Week2Demo;User Id=IT410Login;Password=IT410Password;Trusted_connection=True;"; // Very Unsecure, but local use only

	public static void Main(string[] args)
	{
		Console.Write("Creating Connection For Customers...");
		CustomerRepository customersRepo = new(connection_string);

		Console.WriteLine("Getting All Customers...");
		List<Customer> allCustomers = customersRepo.GetAllCustomers();
		foreach (Customer c in allCustomers)
		{
			Console.WriteLine(c);
		}

		Console.WriteLine("Getting First Customer...");
		Customer? firstCustomer = customersRepo.GetCustomerById(1);


		Console.WriteLine(firstCustomer == null ? "Customer Not Found..." : firstCustomer);

		Console.Write("Creating Connection For Orders...");
		OrderRepository ordersRepo = new(connection_string);

		Console.WriteLine("Getting First Order...");
		Order? firstOrder = ordersRepo.GetOrderById(1);
		
		Console.WriteLine(firstOrder == null ? "Order Not Found..." : firstOrder);

		if (firstOrder != null)
		{
			Console.WriteLine($"Order Before: {firstOrder}");
			Console.Write("Updating Order With New DateTime...");
			firstOrder.OrderDate = DateTime.Now;
			Console.WriteLine($"Order After Change: {firstOrder}");
			Console.Write("Updating Database with new Order change...");
			ordersRepo.UpdateOrder(firstOrder);
			Order? newFirstOrder = ordersRepo.GetOrderById(firstOrder.OrderId);
			Console.WriteLine(newFirstOrder == null ? "Order Not Found..." : newFirstOrder);
			
			Console.WriteLine($"Order Before: {firstOrder}");
			Console.Write("Updating Database with Order Deletion...");
			ordersRepo.DeleteOrder(newFirstOrder.OrderId);
			Order? orderAfterDelete = ordersRepo.GetOrderById(newFirstOrder.OrderId);
			Console.WriteLine(newFirstOrder == null ? "Order Not Found..." : newFirstOrder);
		}
		else
		{
			Console.WriteLine("Order Not found, skipping UPDATE and DELETE examples on Order");
		}

	}
}
