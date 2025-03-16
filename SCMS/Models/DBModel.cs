using MySqlConnector;

namespace SCMS.Models
{
    public class DBModel
    {
        private readonly string _connectionString = "Server=127.0.0.1;User ID=root;Password=Gmysql#4321;Database=scms";

        // Constructor to inject IConfiguration
        /* public DBModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("scmsCon");
        }*/

        public bool IsValidUser(string email, string password)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT password FROM users WHERE email = @Email AND password = @Password";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password); // Consider hashing passwords for security

        string hashedPassword = Convert.ToString(cmd.ExecuteScalar());
            return password == hashedPassword;

       //     return BCrypt.Net.BCrypt.Verify(text: password, hash: hashedPassword);

            // int userCount = Convert.ToInt32(cmd.ExecuteScalar());
            // return userCount > 0;
        }
        
        public bool IsValidEmail(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE email = @Email";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);

            int userCount = Convert.ToInt32(cmd.ExecuteScalar());
            return userCount > 0;
        }

        public bool CreateAdminUser(string username, string nic, string email, string phone, string password)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string query = "CALL InsertUserAndAdmin(@Username, @Nic, @Email, @Phone, @Password)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Nic", nic);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            try
            {
                cmd.ExecuteReader();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool CreateAccStaffUser(string username, string nic, string email, string phone, string password, DateOnly dob, string firstName, string lastName) {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string query = "CALL InsertUserAndAcc(@Username, @Nic, @Email, @Phone, @Password, @Dob, @FirstName, @LastName)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Nic", nic);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            cmd.Parameters.AddWithValue("@Dob", dob);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            try
            {
                cmd.ExecuteReader();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool CreateStudentUser(string username, string nic, string email, string phone, string password, DateOnly dob, string firstName, string lastName) {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string query = "CALL InsertUserAndStu(@Username, @Nic, @Email, @Phone, @Password, @Dob, @FirstName, @LastName)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Nic", nic);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            cmd.Parameters.AddWithValue("@Dob", dob);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            try
            {
                cmd.ExecuteReader();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }
    }
}
