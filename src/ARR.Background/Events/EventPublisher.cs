using System;
using ARR.Data.Entities;
using ARR.Repository;
using Quartz;
using Raven.Client;

namespace ARR.Background.Events
{
    public class EventPublisherJobFactory
    {
        private readonly IDocumentStore _store;
        private readonly IObserver<Event> _reviewSessionMonitor;

        public EventPublisherJobFactory(IDocumentStore store, IObserver<Event> reviewSessionMonitor )
        {
            _store = store;
            _reviewSessionMonitor = reviewSessionMonitor;
        }

        public IJob Instance { get { return new EventPublisher(_store, _reviewSessionMonitor); } }

        public class EventPublisher : Publisher<Event>, IJob
        {
            private readonly IDocumentStore _store;

            public EventPublisher(IDocumentStore store, IObserver<Event> reviewSessionMonitor)
            {
                _store = store;
                Subscribe(reviewSessionMonitor);
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
}