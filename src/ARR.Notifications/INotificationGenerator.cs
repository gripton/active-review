using System.Net.Mail;
using ARR.Data.Entities;

namespace ARR.Notifications
{
    public interface INotificationGenerator
    {
        MailMessage Generate(EventType eventType, NotificationArgs args);

        //MailMessage GenerateReleasedMessage(string recipient, int sessionId, string sessionName);
        //MailMessage GenerateAssignedMessage(string recipient, string sessionName);
        //MailMessage GenerateCompletedMessage(string recipient, string sessionName);
    }
}