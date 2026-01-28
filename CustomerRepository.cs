using System.Data.SqlClient;

public class CustomerRepository
{
	private string _connectionString { get; set; }

	public CustomerRepository(string connection_string)
	{
		_connectionString = connection_string;
	}

	public List<Customer> GetAllCustomers()
	{
		List<Customer> returnList = new();

		using (SqlConnection connection = new(_connectionString))
		{
			connection.Open();

			using (SqlCommand command = new("SELECT CustomerId, FirstName, LastName, Email, IsActive FROM dbo.Customers", connection))
			{
				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						returnList.Add(new Customer
						{
							CustomerId = dataReader.GetInt32(0),
							FirstName = dataReader.GetString(1),
							LastName = dataReader.GetString(2),
							Email = dataReader.GetString(3),
							IsActive = dataReader.GetBoolean(4)
						});
					}
				}
			}
		}

		return returnList;
	}

	public Customer? GetCustomerById(int id)
	{
		Customer? returnCustomer = null;

		using (SqlConnection connection = new(_connectionString))
		{
			connection.Open();

			using (SqlCommand command = new("SELECT CustomerId, FirstName, LastName, Email, IsActive FROM dbo.Customers WHERE CustomerId = @Id", connection))
			{
				command.Parameters.AddWithValue("@Id", id);

				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						returnCustomer = new Customer
						{
							CustomerId = dataReader.GetInt32(0),
							FirstName = dataReader.GetString(1),
							LastName = dataReader.GetString(2),
							Email = dataReader.GetString(3),
							IsActive = dataReader.GetBoolean(4)
						};
					}
				}
			}
			return returnCustomer;
		}
	}


	public bool UpdateCustomer(Customer customer)
	{
		using (SqlConnection conn = new SqlConnection(_connectionString))
		{
			conn.Open();

			using (SqlCommand cmd = new SqlCommand(
				@"UPDATE dbo.Customers
				SET FirstName = @FirstName,
					LastName = @LastName,
					Email = @Email,
					IsActive = @IsActive
				WHERE CustomerId = @CustomerId;", conn))
			{
				cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
				cmd.Parameters.AddWithValue("@LastName", customer.LastName);
				cmd.Parameters.AddWithValue("@Email", customer.Email);
				cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);

				int rows = cmd.ExecuteNonQuery();
				return rows == 1;
			}
		}
	}

	public bool DeleteCustomer(int CustomerId)
	{
		using (SqlConnection conn = new SqlConnection(_connectionString))
		{
			conn.Open();

			using (SqlCommand cmd = new SqlCommand(
				"DELETE FROM dbo.Customers WHERE CustomerId = @CustomerId;", conn))
			{
				cmd.Parameters.AddWithValue("@CustomerId", CustomerId);

				int rows = cmd.ExecuteNonQuery();
				return rows == 1;
			}
		}
	}
}