using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ARR.Prototype.API.Controllers
{
    public interface INotificationGenerator
    {
        MailMessage CreateContentAddedNotification();
    }

    public class ReviewSessionNotificationGenerator : INotificationGenerator
    {
        public MailMessage CreateContentAddedNotification()
        {
            var message = new MailMessage("thorfio@gmail.com", "thorfio@gmail.com");
            message.Body = "This is a test of the active review notification system";
            return message;
        }
    }
}