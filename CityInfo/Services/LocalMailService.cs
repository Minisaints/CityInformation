using System.Diagnostics;

namespace CityInfo.Services
{
    public class LocalMailService : IMailService
    {

        public string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        public string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom}  to {_mailTo}, with LocalMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");

        }
    }
}
