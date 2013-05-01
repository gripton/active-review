using ARR.Data.Entities;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Repository
{
    public class ReviewSessionRepository : AbstractRepository<ReviewSession>
    {
        public ReviewSessionRepository(IDocumentSession session) : base(session) { }

        public override ReviewSession GetByName(string name)
        {
            return Find((r) => r.Title == name).FirstOrDefault();
        }

        protected override void InitializePatchFunctions()
        {
            
        }
    }
}
