using MySqlConnector;

namespace SCMS.Models
{
    public class DBModule
    {

        private readonly string _connectionString;       
        private readonly MySqlConnection connection;

        // Constructor to inject IConfiguration
        public DBModule()
        {
            // _connectionString = configuration.GetConnectionString("scmsCon");
            _connectionString  = "Server=127.0.0.1;User ID=root;Password=Gmysql#4321;Database=scms";
            connection = new MySqlConnection(_connectionString);

        }

        public List<ModuleModel> GetModules()
        {
            List<ModuleModel> lst = new List<ModuleModel>();

            string query = "SELECT * FROM modules";
            using var cmd = new MySqlConnector.MySqlCommand(query, connection);

            try
            {
                connection.Open();
                using var reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    lst.Add(new ModuleModel(
                                reader.GetString("id"),
                                reader.GetString("name"),
                                int.Parse(reader.GetInt64("lecture").ToString(), System.Globalization.CultureInfo.CurrentCulture)
                                ));
                }

                return lst;
            }
            catch
            {
                return lst;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
