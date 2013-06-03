using ARR.Data.Entities;

using Raven.Client;

namespace ARR.Repository
{
    public class EventRepository : AbstractRepository<Event>
    {
        public EventRepository(IDocumentSession session) : base(session) { }

        public EventRepository(IDocumentStore store) : base(store) { }
    }
}
