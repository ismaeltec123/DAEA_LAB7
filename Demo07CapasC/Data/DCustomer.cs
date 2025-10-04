using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entity;
using Microsoft.Data.SqlClient;

namespace Data
{
    public class DCustomer
    {
        // Ajusta con tu servidor/DB (copié el estilo de tu screenshot)
        private readonly string _connectionString =
            "Server=LAB411-021\\SQLEXPRESS;Database=DAE;" +
            "Integrated Security=true;TrustServerCertificate=true";

        public List<Customer> Read()
        {
            var customers = new List<Customer>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_ListarClientes_Activos", connection)) // ← aquí
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerId = Convert.ToInt32(reader["customer_id"]),
                            Name = reader["name"].ToString(),
                            Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader["address"].ToString(),
                            Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader["phone"].ToString(),
                            Active = Convert.ToBoolean(reader["active"])
                        });
                    }
                }
            }

            return customers;
        }
        public void Create(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("InsertCustomer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@name", customer.Name);

                // Manejo de NULLs
                command.Parameters.Add("@address", SqlDbType.NVarChar, 255)
                       .Value = (object)customer.Address ?? DBNull.Value;

                command.Parameters.Add("@phone", SqlDbType.NVarChar, 15)
                       .Value = (object)customer.Phone ?? DBNull.Value;

                command.Parameters.AddWithValue("@active", customer.Active);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Customer c)
        {
            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UpdateCustomer", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@customer_id", c.CustomerId);
                cmd.Parameters.AddWithValue("@name", c.Name);

                cmd.Parameters.Add("@address", SqlDbType.NVarChar, 255)
                   .Value = (object)c.Address ?? DBNull.Value;

                cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 15)
                   .Value = (object)c.Phone ?? DBNull.Value;

                cmd.Parameters.AddWithValue("@active", c.Active);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void SoftDelete(int customerId)
        {
            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SoftDeleteCustomer", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customer_id", customerId);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
