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
        public AccountMonitor(ReviewSessionRepository repository, INotificationGenerator generator, INotificationSender sender)
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
            switch (evt.EventType)
            {
                case EventType.ReviewerInvited:
                    var invitee = evt.Info["invitee"];
                    InviteReviewer(invitee);
                    evt.Recevied = true;
                    break;
                case EventType.InviteeRegistered:
                    var reviewId = evt.EntityId;
                    var username = evt.Info["invitee"];
                    AssignInvitedReviewer(reviewId, username);
                    evt.Recevied = true;
                    break;
                default:
                    return;
            }
        }
    }
}
