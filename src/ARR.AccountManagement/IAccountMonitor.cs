using ARR.Data.Entities;
using ARR.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.AccountManagement
{
    public interface IAccountMonitor : IEventProcessor
    {
        void InviteReviewer(string invitee);
        void AssignInvitedReviewer(int reviewId, string username);
    }
}
