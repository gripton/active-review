using System;
using System.Threading.Tasks;

using ARR.Data.Entities;
using ARR.Repository;
using NLog;
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

        [DisallowConcurrentExecution]
        public class EventPublisher : Publisher<Event>, IJob
        {
            private readonly IDocumentStore _store;
            private static readonly Logger log = LogManager.GetLogger(typeof(EventPublisher).Name);

            public EventPublisher(IDocumentStore store, IObserver<Event> reviewSessionMonitor)
            {
                _store = store;
                Subscribe(reviewSessionMonitor);
            }

            public void Execute(IJobExecutionContext context)
            {
                log.Info("EventPublisher is executing");

                var eventRepository = new EventRepository(_store.OpenSession());
                var events = eventRepository.ListAll();

                // Process the new events
                Parallel.ForEach(events, Notify);

                // Delete the recevied events
                Parallel.ForEach(events, evt => { if (evt.Recevied) eventRepository.Delete(evt); });
            }
        }
    }
}