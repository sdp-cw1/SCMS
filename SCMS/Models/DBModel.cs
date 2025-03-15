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

            string query = "SELECT password FROM users WHERE email = @Email AND password = @Password";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password); // Consider hashing passwords for security

            string hashedPassword = Convert.ToString(cmd.ExecuteScalar());

            if (hashedPassword.Length == 0)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(text: password, hash: hashedPassword);
        }
    }
}
