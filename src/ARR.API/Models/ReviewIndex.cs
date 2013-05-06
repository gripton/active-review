using ARR.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.API.Models
{
    public class ReviewIndex
    {
        public int Id { get; set; }
        public string Reviewer { get; set; }
        public string Title { get; set; }
        public DateTime LastModified { get; set; }
        public SessionStatusType SessionStatus { get; set; }
    }
}