﻿using System;
using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public enum SessionStatusType
    {
        Created = 0,
        Released = 1,
        Archived = 2
    }

    public class ReviewSession
    {
        // NOTE: Courtenay to consider handling 'invitee' scenario
        public string Creator { get; set; }
        public string Reviewer { get; set; }
        public string Title { get; set; }
       
        public DateTime LastModified { get; set; }

        public List<Question> Questions { get; set; }
        public List<Requirement> Requirements { get; set; }

        public SessionStatusType SessionStatus { get; set; }
    }
}
