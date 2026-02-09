using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // Add this using directive

namespace Week2AdoNetDemo
{

    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<Product> GetAll()
        {
            List<Product> results = new List<Product>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT ProductId,
                     ProductName,
                     CategoryId,
                     UnitPrice,
                     IsActive
              FROM dbo.Products;", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product p = new Product
                            {
                                ProductId = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                CategoryId = reader.GetInt32(2),
                                UnitPrice = reader.GetDecimal(3),
                                IsActive = reader.GetBoolean(4)
                            };

                            results.Add(p);
                        }
                    }
                }
            }

            return results;
        }

        public Product GetById(int id)
        {
            Product result = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT ProductId,
                     ProductName,
                     CategoryId,
                     UnitPrice,
                     IsActive
              FROM dbo.Products
              WHERE ProductId = @Id;", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Product
                            {
                                ProductId = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                CategoryId = reader.GetInt32(2),
                                UnitPrice = reader.GetDecimal(3),
                                IsActive = reader.GetBoolean(4)
                            };
                        }
                    }
                }
            }

            return result;
        }

        public bool UpdateProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE dbo.Products
              SET ProductName = @Name,
                  CategoryId = @CategoryId,
                  UnitPrice = @Price,
                  IsActive = @IsActive
              WHERE ProductId = @Id;", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", product.ProductName);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@Price", product.UnitPrice);
                    cmd.Parameters.AddWithValue("@IsActive", product.IsActive);
                    cmd.Parameters.AddWithValue("@Id", product.ProductId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows == 1;
                }
            }
        }

        public bool DeleteProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Products WHERE ProductId = @Id;", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows == 1;
                }
            }
        }


        public void UpdateAndDeactivateProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();

                try
                {
                    SqlCommand updateCmd = new SqlCommand(
                        @"UPDATE dbo.Products
                  SET ProductName = @Name,
                      UnitPrice = @Price
                  WHERE ProductId = @Id;", conn, tx);

                    updateCmd.Parameters.AddWithValue("@Name", product.ProductName);
                    updateCmd.Parameters.AddWithValue("@Price", product.UnitPrice);
                    updateCmd.Parameters.AddWithValue("@Id", product.ProductId);
                    updateCmd.ExecuteNonQuery();

                    SqlCommand deactivateCmd = new SqlCommand(
                        @"UPDATE dbo.Products
                  SET IsActive = 0
                  WHERE ProductId = @Id;", conn, tx);

                    deactivateCmd.Parameters.AddWithValue("@Id", product.ProductId);
                    deactivateCmd.ExecuteNonQuery();

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
}