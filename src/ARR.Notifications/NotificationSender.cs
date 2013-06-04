using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace ARR.Notifications
{
    public class NotificationSender : INotificationSender
    {
        public void SendNotification(MailMessage message)
        {
            var server = ConfigurationManager.AppSettings["SMTPServer"];
            var port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            var username = ConfigurationManager.AppSettings["SMTPUsername"];
            var password = ConfigurationManager.AppSettings["SMTPPassword"];

            try
            {
                using (var client = new SmtpClient(server, port))
                {
                    client.Credentials = new NetworkCredential(username, password);
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                string test = e.Message;
                throw;
            }
            
        }
    }
}
