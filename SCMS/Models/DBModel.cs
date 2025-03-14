namespace SCMS.Models
{
    public class DBModel
    {
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public bool isValidUser(string email, string password)
        {
            // TODO
        }
    }
}
