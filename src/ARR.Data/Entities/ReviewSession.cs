﻿using System;
using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public enum SessionStatusType
    {
        Created = 0,
        Released = 1,
        Completed = 2,
        Archived = 3
    }

    public class ReviewSession : IPersistentEntity
    {
        public const string SaveQuestionnairePatch = "save-questionnaire";

        public int Id { get; set; }
        public string Creator { get; set; }
        public string Reviewer { get; set; }
        public string Title { get; set; }
        public bool PendingReviewer { get; set; }
        public DateTime LastModified { get; set; }

        public List<Question> Questions { get; set; }
        public List<Requirement> Requirements { get; set; }

        public SessionStatusType SessionStatus { get; set; }
    }
}
