using ARR.Data.Entities;
using ARR.Repository;

using Quartz;
using Raven.Client;

namespace ARR.Background.Events
{
    public class EventPublisher : Publisher<Event>, IJob
    {
        private readonly IDocumentStore _store;

        public EventPublisher(IDocumentStore store)
        {
            _store = store;
        }

        public void Execute(IJobExecutionContext context)
        {
            var eventRepository = new EventRepository(_store.OpenSession());
            var events = eventRepository.FindAll();

            foreach (var evt in events)
            {
                Notify(evt);
            }
        }
    }
}