using ARR.Data.Entities;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Repository
{
    public interface IEventProcessor
    {
        void ProcessEvent(Event evt);
    }

    public abstract class AbstractEventSubscriber
    {
        private readonly IDocumentStore _store;
        private readonly IEventProcessor _eventProcessor;

        protected AbstractEventSubscriber(IDocumentStore store, IEventProcessor eventProcessor)
        {
            _store = store;
            _eventProcessor = eventProcessor;
        }

        public void Subscribe()
        {
            EventObserver = new SubscriptionEventHandler(_store, Receive);

            _store.Changes()
                .ForDocumentsStartingWith("events")
                .Subscribe(EventObserver);
        }

        protected SubscriptionEventHandler EventObserver { get; set; }

        public void Receive(Event evt)
        {
            _eventProcessor.ProcessEvent(evt);
            
        }  
    }
    
    public class SubscriptionEventHandler : IObserver<DocumentChangeNotification>
    {
        private readonly IDocumentStore _store;
        private readonly Action<Event> _received;
        
        public SubscriptionEventHandler(IDocumentStore store, Action<Event> received)
        {
            _store = store;
            _received = received;
        }

        public void OnCompleted() 
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DocumentChangeNotification value)
        {
            if (value.Type == DocumentChangeTypes.Put)
            {
                HandleEvent(int.Parse(value.Id));
            }
        }

        private void HandleEvent(int eventId)
        {
            using (var session = _store.OpenSession())
            {
                var evt = session.Load<Event>(eventId);
                
                try
                {
                    _received.Invoke(evt);
                }
                catch (Exception)
                {
                    // Deal with it
                    throw;
                }

                if (evt.Recevied) session.Delete<Event>(evt);
            }            
        }        
    }

    

    //public class ReviewSessionMonitor : AbstractEventMonitor
    //{
    //    public ReviewSessionMonitor(IDocumentStore store) : base(store) { }

    //    protected override bool PickedUp(int id, EventType eventType)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
