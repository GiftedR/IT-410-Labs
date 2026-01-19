internal class Program
{
	const string connection_string = $"Server=localhost;Database=IT410Week2Demo;User Id=IT410Login;Password=IT410Password;Trusted_connection=True;"; // Very Unsecure, but local use only

	public static void Main(string[] args)
	{
		Console.Write("Creating Connection...");
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
	}
}
