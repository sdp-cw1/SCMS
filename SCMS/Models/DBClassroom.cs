using MySqlConnector;

namespace SCMS.Models
{
    public class DBClassroom
    {

        private readonly string _connectionString;       
        private readonly MySqlConnection connection;

        // Constructor to inject IConfiguration
        public DBClassroom()
        {
            // _connectionString = configuration.GetConnectionString("scmsCon");
            _connectionString  = "Server=127.0.0.1;User ID=root;Password=Gmysql#4321;Database=scms";
            connection = new MySqlConnection(_connectionString);

        }

        public bool InsertClassroom(string id, string name, int seats)
        {
            string query = "INSERT INTO classrooms VALUES (@Id, @Name, @Seats)";
            using var cmd = new MySqlConnector.MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Seats", seats);

            try
            {
                connection.Open();
                cmd.ExecuteScalar();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<ClassRoomModel> GetClassRooms()
        {
            List<ClassRoomModel> lst = new List<ClassRoomModel>();

            string query = "SELECT * FROM classrooms";
            using var cmd = new MySqlConnector.MySqlCommand(query, connection);

            try
            {
                connection.Open();
                using var reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    lst.Add(new ClassRoomModel(
                                reader.GetString("id"),
                                reader.GetString("name"),
                                int.Parse(reader.GetInt64("seats").ToString(), System.Globalization.CultureInfo.CurrentCulture)
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
