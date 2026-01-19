public class CustomerRepository
{
	private string _connectionString { get; set; }

	public CustomerRepository(string connection_string)
	{
		_connectionString = connection_string;
	}

	public List<Customer> GetAllCustomers()
	{
		return [];
	}

	~CustomerRepository()
	{
		
	}
}