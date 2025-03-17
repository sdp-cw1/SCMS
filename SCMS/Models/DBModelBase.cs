namespace SCMS.Models
{
    public class DBModelBase
    {
        private readonly string _smtpAppPassword = "hsccoxszqfuaxxbr"; // Your app password for Gmail SMTP

        internal bool SendPasswordEmail(string email, string generatedPassword)
        {
            throw new NotImplementedException();
        }
    }
}