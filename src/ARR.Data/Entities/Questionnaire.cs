using System;
using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public class Questionnaire
    {
        public DateTime Created { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public string Content { get; set; }
        public List<Feedback> Feedback { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public string Text { get; set; }
    }
}