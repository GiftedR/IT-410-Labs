using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2AdoNetDemo
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        // We expose this so Program.cs can print it after a run
        public int CommandCount { get; private set; }

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlCommand CreateCountedCommand(string sql, SqlConnection conn)
        {
            CommandCount++;
            return new SqlCommand(sql, conn);
        }

        private SqlCommand CreateCountedCommand(string sql, SqlConnection conn, SqlTransaction tx)
        {
            CommandCount++;
            return new SqlCommand(sql, conn, tx);
        }

        public List<Order> GetOrdersWithItems_NPlusOne()
        {
            CommandCount = 0;

            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1) Load all orders (1 query)
                using (SqlCommand cmd = CreateCountedCommand(@"
                    SELECT OrderId, CustomerId, OrderDate, OrderStatus
                    FROM dbo.Orders
                    ORDER BY OrderId;", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = reader.GetInt32(0),
                                CustomerId = reader.GetInt32(1),
                                OrderDate = reader.GetDateTime(2),
                                OrderStatus = reader.GetString(3)
                            });
                        }
                    }
                }

                // 2) For each order, load items (N queries)
                string sqlItems = @"
                    SELECT OrderItemId, OrderId, ProductId, Quantity, UnitPrice
                    FROM dbo.OrderItems
                    WHERE OrderId = @OrderId
                    ORDER BY OrderItemId;";

                foreach (Order o in orders)
                {
                    using (SqlCommand cmdItems = CreateCountedCommand(sqlItems, conn))
                    {
                        cmdItems.Parameters.AddWithValue("@OrderId", o.OrderId);

                        using (SqlDataReader r2 = cmdItems.ExecuteReader())
                        {
                            while (r2.Read())
                            {
                                o.Items.Add(new OrderItem
                                {
                                    OrderItemId = r2.GetInt32(0),
                                    OrderId = r2.GetInt32(1),
                                    ProductId = r2.GetInt32(2),
                                    Quantity = r2.GetInt32(3),
                                    UnitPrice = r2.GetDecimal(4)
                                });
                            }
                        }
                    }
                }
            }

            return orders;
        }

        public List<Order> GetOrdersWithItems_Join()
        {
            CommandCount = 0;

            Dictionary<int, Order> orders = new Dictionary<int, Order>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = CreateCountedCommand(@"
                    SELECT 
                        o.OrderId, o.CustomerId, o.OrderDate, o.OrderStatus,
                        oi.OrderItemId, oi.ProductId, oi.Quantity, oi.UnitPrice
                    FROM dbo.Orders o
                    LEFT JOIN dbo.OrderItems oi ON o.OrderId = oi.OrderId
                    ORDER BY o.OrderId, oi.OrderItemId;", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(0);

                            if (!orders.TryGetValue(orderId, out Order order))
                            {
                                order = new Order
                                {
                                    OrderId = orderId,
                                    CustomerId = reader.GetInt32(1),
                                    OrderDate = reader.GetDateTime(2),
                                    OrderStatus = reader.GetString(3)
                                };

                                orders.Add(orderId, order);
                            }

                            // If there's no OrderItem row (LEFT JOIN), skip item creation
                            if (!reader.IsDBNull(4))
                            {
                                order.Items.Add(new OrderItem
                                {
                                    OrderItemId = reader.GetInt32(4),
                                    OrderId = orderId,
                                    ProductId = reader.GetInt32(5),
                                    Quantity = reader.GetInt32(6),
                                    UnitPrice = reader.GetDecimal(7)
                                });
                            }
                        }
                    }
                }
            }

            return new List<Order>(orders.Values);
        }
    }
}
