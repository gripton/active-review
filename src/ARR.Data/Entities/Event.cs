using System;
using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public enum EventType
    {
        ReviewerInvited = 0,
        InviteeRegistered = 1,
        FeedbackProvided = 2,
        FeedbackAcknowleged = 3,
        QuestionnaireCompleted = 4,
        ReviewReleased = 5
    }

    public class Event : IPersistentEntity
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public DateTime Created { get; set; }
        public EventType EventType { get; set; }
        public Dictionary<string,string> Info { get; set; }
        public bool Recevied { get; set; }         
    }
}
