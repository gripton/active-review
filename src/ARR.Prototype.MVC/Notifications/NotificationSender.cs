using System.Net;
using System.Net.Mail;

namespace ARR.Prototype.MVC.Notifications
{
    public class NotificationSender
    {
        public void Send()
        {
            var message = new MailMessage("thorfio@gmail.com", "thorfio@gmail.com");
            message.Body = "This is a test of the active review notification system";

            using(var client = new SmtpClient("smtp.mailgun.org", 587))
            {
                client.Credentials = new NetworkCredential("postmaster@app14359.mailgun.org", "7dq2bp-1mi50");
                client.Send(message);
            }
        }
    }
}