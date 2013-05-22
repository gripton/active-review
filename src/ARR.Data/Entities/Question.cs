using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public class Question
    {
        public Question()
        {
            Feedbacks = new List<Feedback>();
        }

        public string Content { get; set; }        
        public string Answer { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }    
}
