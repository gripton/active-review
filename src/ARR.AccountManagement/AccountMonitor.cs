using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;
using System;

namespace ARR.AccountManagement
{
    public class AccountMonitor : IAccountMonitor
    {
        public AccountMonitor(ReviewSessionRepository repository,  INotificationGenerator generator, INotificationSender sender)
        {

        }

        public void InviteReviewer(string invitee)
        {
            throw new NotImplementedException();
        }

        public void AssignInvitedReviewer(int reviewId, string username)
        {
            throw new NotImplementedException();
        }

        public void ProcessEvent(Event evt)
        {
            
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Event value)
        {
            
        }
    }
}
