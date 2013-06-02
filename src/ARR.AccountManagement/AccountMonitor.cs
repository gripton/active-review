using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            switch (value.EventType)
            {
                case EventType.ReviewerInvited:
                    var invitee = value.Info["invitee"];
                    InviteReviewer(invitee);
                    value.Recevied = true;
                    break;
                case EventType.InviteeRegistered:
                    var reviewId = value.EntityId;
                    var username = value.Info["invitee"];
                    AssignInvitedReviewer(reviewId, username);
                    value.Recevied = true;
                    break;
                default:
                    return;
            }
        }
    }
}
