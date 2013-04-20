using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ARR.Prototype.API.Controllers
{
    public interface INotificationSender
    {
        void SendNotification(MailMessage message);
    }
    
    public class NotificationSender : INotificationSender
    {
        public void SendNotification(MailMessage message)
        {
            using (var client = new SmtpClient("smtp.mailgun.org", 587))
            {
                client.Credentials = new NetworkCredential("postmaster@app14359.mailgun.org", "7dq2bp-1mi50");
                client.Send(message);
            }
        }
    }
}