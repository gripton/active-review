using ARR.Data.Entities;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Repository
{
    public class EventRepository : AbstractRepository<Event>
    {
        public EventRepository(IDocumentSession session) : base(session) { }
    }
}
