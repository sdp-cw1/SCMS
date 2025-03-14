using System;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace SCMS.Models
{
    public class DBModel
    {
        private readonly string _connectionString;

        // Constructor to inject IConfiguration
        public DBModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("scmsCon");
        }

        public bool IsValidUser(string email, string password)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE email = @Email AND password = @Password";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password); // Consider hashing passwords for security

            int userCount = Convert.ToInt32(cmd.ExecuteScalar());
            return userCount > 0;
        }
    }
}
