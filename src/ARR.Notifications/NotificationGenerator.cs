using System.Net.Mail;

namespace ARR.Notifications
{
    public class NotificationGenerator : INotificationGenerator
    {
        private const string releasedSubject = "Review Session Released";
        private const string releasedMessage = 
            "Greetings. Review Session '{0}' has been released for particpation. Click here to begin the Questionnaire.";

        private const string assignedSubject = "Reviewer Assigned";
        private const string assignedMessage =
            "Greetings. You have been assigned as the reviewer for Active Review Session '{0}'. Once the Review " +
            "Session has been released you will reveice another notification with a link to the Review Session Questionnaire.";

        private const string completedSubject = "Review Session Questionnaire Completed";
        private const string completedMessage =
            "Greetings. The Questionnaire for Review Session '{0}' has been completed by the reviewer.";



        public MailMessage GenerateReleasedMessage(string recipient, int sessionId, string sessionName)
        {
            var message = GenerateMessage(recipient, releasedSubject, string.Format(releasedMessage, sessionName));
            return message;
        }

        public MailMessage GenerateAssignedMessage(string recipient, string sessionName)
        {
            var message = GenerateMessage(recipient, releasedSubject, string.Format(releasedMessage, sessionName));
            return message;
        }

        public MailMessage GenerateCompletedMessage(string recipient, string sessionName)
        {
            var message = GenerateMessage(recipient, releasedSubject, string.Format(releasedMessage, sessionName));
            return message;
        }

        private MailMessage GenerateMessage(string recipient, string subject, string body)
        {
            var message = new MailMessage("noreply@activerequirementsreview.apphb.com", recipient)
            {
                Subject = string.Format("Active Requirements Review - {0}", subject),
                Body = body
            };

            return message;
        }
    }
}