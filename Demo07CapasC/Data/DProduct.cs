using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Collections;

namespace Data
{
    public class DProduct
    {

        public  string _connectionString = "Server=LAB411-021\\SQLEXPRESS;Database=DAE;" +
            "Integrated Security=true; TrustServerCertificate=true";
        public List<Product> Read()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("sp_ListarProductos", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            Name = reader["name"].ToString(),
                            Price = Convert.ToDecimal(reader["price"]),
                            Stock = Convert.ToInt32(reader["stock"]),
                            Active = Convert.ToBoolean(reader["active"])
                        };
                        products.Add(product);
                    }
                }
            }

            return products;
        }
        public void Create(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("InsertProduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
