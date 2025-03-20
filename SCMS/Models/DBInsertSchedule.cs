using MySqlConnector;

class DBInsertSchedule
{
        
    private readonly string _connectionString;       
    private readonly MySqlConnection connection;

    // Constructor to inject IConfiguration
    public DBInsertSchedule()
    {
        // _connectionString = configuration.GetConnectionString("scmsCon");
        _connectionString  = "Server=127.0.0.1;User ID=root;Password=Gmysql#4321;Database=scms";
        connection = new MySqlConnection(_connectionString);

    }

    public bool CreateSchedule(DateTime StartDateTime, DateTime endDateTime, string Location, string eventName, string category, int organiser, string module)
    {
        string query = "CALL InsertSchedule(@EventName, @Category, @Organiser, @StartDateTime, @EndDateTime, @Location)";
        using var cmd = new MySqlConnector.MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@StartDateTime", StartDateTime);
        cmd.Parameters.AddWithValue("@EndDateTime", endDateTime);
        cmd.Parameters.AddWithValue("@Location", "LO0001");
        cmd.Parameters.AddWithValue("@EventName", eventName);
        cmd.Parameters.AddWithValue("@Category", category);
        cmd.Parameters.AddWithValue("@Organiser", organiser);
        // cmd.Parameters.AddWithValue("@Module", module);

        try
        {
            connection.Open();
            var rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
    }


    public List<ScheduleModel> GetScheduleList(int userId, DateOnly StartDate, DateOnly EndDate)
    {
        string query = @"
            SELECT 
            s.id,
            e.id,
            e.name,
            s.start_date,
            s.end_date,
            s.location,
            e.category
            FROM schedules s
            INNER JOIN events e ON s.event_id = e.id
            INNER JOIN participants p ON s.id = p.schedule_id
            WHERE s.start_date >= @StartDate 
            AND s.start_date <= @EndDate 
            AND p.user_id = @UserId
        ";
        // conditions to check
        // schedule ids where user.id in participants table
        // must return schedule info: schedules.id, schedules.start_date, schedules.end_date, event_id -> events.id, events.category, 

        using var cmd = new MySqlConnector.MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@StartDate", StartDate);
        cmd.Parameters.AddWithValue("@EndDate", EndDate);
        cmd.Parameters.AddWithValue("@UserId", userId);
        using var reader = cmd.ExecuteReader();

        var scheduleList = new List<ScheduleModel>();

        int scheduleIdIndex = reader.GetOrdinal("id");
        int eventIdIndex = reader.GetOrdinal("event_id");
        int eventNameIndex = reader.GetOrdinal("name");
        int startDateIndex = reader.GetOrdinal("start_date");
        int endDateIndex = reader.GetOrdinal("end_date");
        int locationIndex = reader.GetOrdinal("location");
        int categoryIndex = reader.GetOrdinal("category");

        while (reader.Read())
        {
            scheduleList.Add(new ScheduleModel(
                        reader.GetString(scheduleIdIndex),
                        reader.GetString(eventNameIndex),
                        reader.GetString(eventNameIndex),
                        reader.GetDateTime(startDateIndex),
                        reader.GetDateTime(endDateIndex),
                        reader.GetString(locationIndex),
                        reader.GetString(categoryIndex)
                        ));
        }
        return scheduleList;
    }

    public bool JoinEvent(string eventId, int userId)
    {
        return false;
    }
}
