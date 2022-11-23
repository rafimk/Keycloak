using System.Security.Cryptography.X509Certificates;
namespace Keycloak.Helper.Notification
{
    public class Notification
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Severity { get; set; }
        public IList<string> Recipients { get; set; }
    }

    public enum NotificationSeverity
    {
        INFO = 1,
        WARNING = 2,
        CRITICAL = 3
    }
}