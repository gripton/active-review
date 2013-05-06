using ARR.AccountManagement;
using ARR.Repository;
using Autofac;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.API.Infrastructure
{
    public class AccountEventSubscriber : AbstractEventSubscriber, IStartable
    {
        public AccountEventSubscriber(IDocumentStore store, IAccountMonitor monitor, AccountRepository accountRepo, ReviewSessionRepository reviewRepo  ) : base(store, monitor) { }
        
        void IStartable.Start()
        {
            this.Subscribe();
        }
    }
}