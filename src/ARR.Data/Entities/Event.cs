using System;
using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public enum EventType
    {
        ReviewerAssigned = 0,
        QuestionnaireCompleted = 1,
        ReviewSessionReleased = 2
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
