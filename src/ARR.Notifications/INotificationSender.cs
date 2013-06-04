using System;
using System.Net.Mail;

namespace ARR.Notifications
{
    public interface INotificationSender
    {
        void SendNotification(MailMessage message);
    }
}