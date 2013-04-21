using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public class ReviewSession
    {
        public string Name { get; set; }
        public Account Creator { get; set; }
        public Account Reviewer { get; set; }
        public List<Question> Questions { get; set; }
        public List<Requirement> Requirements { get; set; }
        public List<Feedback> Forum { get; set; }
    }
}
