using ARR.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.Prototype.API.Models
{
    public class ReviewIndex
    {
        // NOTE: Courtenay to consider handling 'invitee' scenario
        int Id { get; set; }
        public string Reviewer { get; set; }
        public string Title { get; set; }
        public DateTime LastModified { get; set; }
        public SessionStatusType SessionStatus { get; set; }
    }
}