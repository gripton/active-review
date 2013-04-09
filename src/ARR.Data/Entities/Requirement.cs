using System;
using System.Collections.Generic;

namespace ARR.Data.Entities
{
    public class Requirement
    {
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
    }
}