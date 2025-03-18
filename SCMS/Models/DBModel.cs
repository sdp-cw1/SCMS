using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace SCMS.Models
{
    public class DBModel : DBModelBase
    {
        private readonly string _connectionString = "Server=localhost;User ID=root;Password=Gmysql#4321;Database=scms";
        private readonly string _smtpEmail = "ganidujayasanka@gmail.com"; // Use your email here


        // Constructor to inject IConfiguration
        /* public DBModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("scmsCon");
        }*/

        // Method to validate user by email and password
        public bool IsValidUser(string email, string password)
        {
            using var connection = new MySqlConnector.MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT password FROM users WHERE email = @Email AND password = @Password";

            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password); // Consider hashing passwords for security

            string hashedPassword = Convert.ToString(cmd.ExecuteScalar());
            return password == hashedPassword;

            // return BCrypt.Net.BCrypt.Verify(text: password, hash: hashedPassword);
        }

        // Method to check if the email is already registered
        public bool IsValidEmail(string email)
        {
            using var connection = new MySqlConnector.MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM RegisteredStudents WHERE email = @Email";

            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);

            int userCount = Convert.ToInt32(cmd.ExecuteScalar());
            return userCount > 0;
        }

        // Method to create Admin User with hashed password
        public bool CreateAdminUser(string username, string nic, string email, string phone, string password)
        {
            using var connection = new MySqlConnector.MySqlConnection(_connectionString);
            connection.Open();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string query = "CALL InsertUserAndAdmin(@Username, @Nic, @Email, @Phone, @Password)";

            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
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

        // Method to create Account Staff User with hashed password
        public bool CreateAccStaffUser(string username, string nic, string email, string phone, string password, DateOnly dob, string firstName, string lastName)
        {
            using var connection = new MySqlConnector.MySqlConnection(_connectionString);
            connection.Open();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string query = "CALL InsertUserAndAcc(@Username, @Nic, @Email, @Phone, @Password, @Dob, @FirstName, @LastName)";

            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
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

        // Method to create Student User with hashed password
        public bool CreateStudentUser(string username, string nic, string email, string phone, string password, DateOnly dob, string firstName, string lastName)
        {
            using var connection = new MySqlConnector.MySqlConnection(_connectionString);
            connection.Open();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string query = "CALL InsertUserAndStu(@Username, @Nic, @Email, @Phone, @Password, @Dob, @FirstName, @LastName)";

            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
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

        // Method to save the generated password and send email
        public bool SaveGeneratedPasswordToDB(string email, string generatedPassword)
        {
            using var connection = new MySqlConnector.MySqlConnection(_connectionString);
            connection.Open();

            // Use UPDATE query to update AutoGenPassword if the email exists in the table
            string query = "UPDATE RegisteredStudents SET AutoGenPassword = @AutoGenPassword WHERE email = @Email";

            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@AutoGenPassword", generatedPassword);

            try
            {
                // Execute the update query
                int rowsAffected = cmd.ExecuteNonQuery();

                // Check if any rows were updated
                if (rowsAffected > 0)
                {
                    SendPasswordEmail(email, generatedPassword); // Send the generated password to the user via email
                    return true;
                }
                else
                {
                    Console.WriteLine("No matching email found to update.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating AutoGenPassword: " + ex.Message);  // Log the error
                return false;
            }
        }


        // Method to send email with the generated password
        private bool SendPasswordEmail(string emailAddress, string generatedPassword)
        {
            try
            {
                Console.WriteLine($"📧 Sending email to: {emailAddress} with password: {generatedPassword}");

                string smtpEmail = "ganidujayasanka@gmail.com";
                string smtpAppPassword = "hsccoxszqfuaxxbr";

                if (string.IsNullOrEmpty(emailAddress))
                {
                    Console.WriteLine("❌ Recipient email is missing.");
                    return false;
                }

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(smtpEmail),
                    Subject = "Your Generated Password",
                    Body = $"Dear User,\n\nHere is your temporary password: {generatedPassword}\n\nUse this password to log in.\n\nBest Regards,\nSCMS Team"
                };
                mail.To.Add(emailAddress);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(smtpEmail, smtpAppPassword),
                    EnableSsl = true
                };

                smtpClient.Send(mail);
                Console.WriteLine($"✅ Email sent successfully to {emailAddress}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error sending email: {ex.Message}");
                return false;
            }
        }



        // Method to generate a random password
        public string GenerateRandomPassword(int length = 8)
        {
            var random = new Random();

            // Define character sets
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string symbols = "$#@";

            // Generate each required part of the password
            string firstTwoLetters = new string(Enumerable.Range(0, 2).Select(_ => upperCase[random.Next(upperCase.Length)]).ToArray());
            string fourNumbers = new string(Enumerable.Range(0, 4).Select(_ => numbers[random.Next(numbers.Length)]).ToArray());
            char specialSymbol = symbols[random.Next(symbols.Length)];
            char lastLetter = lowerCase[random.Next(lowerCase.Length)];

            // Concatenate the parts
            string password = firstTwoLetters + fourNumbers + specialSymbol + lastLetter;

            return password;
        }

        // Method to validate the temporary password
        public bool ValidateTempPassword(string email, string password)
        {
            using var connection = new MySqlConnector.MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM RegisteredStudents WHERE email = @Email AND AutoGenPassword = @Password";

            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            int matchCount = Convert.ToInt32(cmd.ExecuteScalar());

            return matchCount > 0; // Return true if email and temp password match
        }


    }
}
