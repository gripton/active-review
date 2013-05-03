using Raven.Abstractions.Data;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Repository
{
    public abstract class AbstractEventMonitor
    {
        private readonly IDocumentStore _store;

        protected AbstractEventMonitor(IDocumentStore store)
        {
            _store = store;
        }

        protected void StartMonitoring()
        {
            //_store.Changes()
            //    .ForDocumentsStartingWith("events")
            //    .Subscribe(change =>
            //     {
            //         if (change.Type == DocumentChangeTypes.Put)
            //         {
            //             Console.WriteLine("New user has been added. Its ID is " + change.Id + ", document ETag " + change.Etag);
            //         }
            //     });
        }

        
    }    
}
