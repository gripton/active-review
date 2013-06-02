using System.Net.Mail;

namespace ARR.Notifications
{
    public interface INotificationGenerator
    {
        MailMessage GenerateReleasedMessage(string recipient, int sessionId, string sessionName);
        MailMessage GenerateAssignedMessage(string recipient, string sessionName);
        MailMessage GenerateCompletedMessage(string recipient, string sessionName);
    }
}