using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Data.Entities
{
    public enum EventType
    {
        ReviewerInvited,
        InviteeRegistered,
        FeedbackProvided,
        FeedbackAcknowleged
    }

    public class Event : IPersistentEntity
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public EventType EventType { get; set; }
        public Dictionary<string,string> Info { get; set; } 
    }
}
