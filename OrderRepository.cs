using System.Data.SqlClient;

public class OrderRepository
{
	private string _connectionString { get; set; }

	public OrderRepository(string connection_string)
	{
		_connectionString = connection_string;
	}

	public List<Order> GetAllOrders()
	{
		List<Order> returnList = new();

		using (SqlConnection connection = new(_connectionString))
		{
			connection.Open();

			using (SqlCommand command = new("SELECT OrderId, CustomerId, OrderDate, Status FROM dbo.Orders", connection))
			{
				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						returnList.Add(new Order
						{
							OrderId = dataReader.GetInt32(0),
							CustomerId = dataReader.GetInt32(1),
							OrderDate = dataReader.GetDateTime(2),
							OrderStatus = dataReader.GetString(3)
						});
					}
				}
			}
		}

		return returnList;
	}

	public Order? GetOrderById(int id)
	{
		Order? returnOrder = null;

		using (SqlConnection connection = new(_connectionString))
		{
			connection.Open();

			using (SqlCommand command = new("SELECT OrderId, CustomerId, OrderDate, OrderStatus FROM dbo.Orders WHERE OrderId = @Id", connection))
			{
				command.Parameters.AddWithValue("@Id", id);

				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						returnOrder = new Order
						{
							OrderId = dataReader.GetInt32(0),
							CustomerId = dataReader.GetInt32(1),
							OrderDate = dataReader.GetDateTime(2),
							OrderStatus = dataReader.GetString(3)
						};
					}
				}
			}
			return returnOrder;
		}
	}

	public bool UpdateOrder(Order Order)
	{
		using (SqlConnection conn = new SqlConnection(_connectionString))
		{
			conn.Open();

			using (SqlCommand cmd = new SqlCommand(
				@"UPDATE dbo.Orders
				SET OrderDate = @OrderDate,
					OrderStatus = @OrderStatus
				WHERE OrderId = @OrderId;", conn))
			{
				cmd.Parameters.AddWithValue("@OrderId", Order.OrderId);
				cmd.Parameters.AddWithValue("@OrderDate", Order.OrderDate);
				cmd.Parameters.AddWithValue("@OrderStatus", Order.OrderStatus);

				int rows = cmd.ExecuteNonQuery();
				return rows == 1;
			}
		}
	}

	public bool DeleteOrder(int orderId)
	{
		using (SqlConnection conn = new SqlConnection(_connectionString))
		{
			conn.Open();

			using (SqlCommand cmd = new SqlCommand(
				"DELETE FROM dbo.Orders WHERE OrderId = @OrderId;", conn))
			{
				cmd.Parameters.AddWithValue("@OrderId", orderId);

				int rows = cmd.ExecuteNonQuery();
				return rows == 1;
			}
		}
	}
	public void ChangeOrderStatusAndAddNewItem(Order order, OrderItem orderItem)
	{
		using (SqlConnection conn = new SqlConnection(_connectionString))
		{
			conn.Open();
			SqlTransaction tx = conn.BeginTransaction();

			try
			{
				SqlCommand changeStatus = new SqlCommand(
					@"UPDATE dbo.Orders
					SET OrderStatus = @OrderStatus
					WHERE OrderId = @OrderId;", conn, tx);

				changeStatus.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
				changeStatus.Parameters.AddWithValue("@OrderId", order.OrderId);
				changeStatus.ExecuteNonQuery();

				SqlCommand addItem = new SqlCommand(
					@"INSERT INTO dbo.Orders
					( OrderId, ProductId, Quantity, UnitPrice )
					Values
					( @OrderId, @ProductId, @Quantity, @UnitPrice )", conn, tx);
				
				addItem.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
				addItem.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
				addItem.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
				addItem.Parameters.AddWithValue("@UnitPrice", orderItem.UnitPrice);

				addItem.ExecuteNonQuery();

				tx.Commit();
			}
			catch
			{
				tx.Rollback();
				throw;
			}
		}
	}
}