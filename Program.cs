internal class Program
{
	const string connection_string = $"Server=localhost;Database=IT410Week2Demo;User Id=IT410Login;Password=IT410Password;"; // Very Unsecure, but local use only

	public static void Main(string[] args)
	{
		CustomerRepository customersRepo = new(connection_string);

		List<Customer> allCustomers = customersRepo.GetAllCustomers();

		Customer? firstCustomer = customersRepo.GetCustomerById(1);

		Console.WriteLine();
	}
}
