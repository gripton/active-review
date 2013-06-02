using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace ARR.Notifications
{
    public class NotificationSender : INotificationSender
    {
        public void SendNotification(Func<MailMessage> messageGenerator)
        {
            var server = ConfigurationManager.AppSettings["SMTPServer"];
            var port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            var username = ConfigurationManager.AppSettings["SMTPUsername"];
            var password = ConfigurationManager.AppSettings["SMTPPassword"];

            using (var client = new SmtpClient(server, port))
            {
                var message = messageGenerator.Invoke();
                client.Credentials = new NetworkCredential(username, password);
                client.Send(message);
            }
        }
    }
}
