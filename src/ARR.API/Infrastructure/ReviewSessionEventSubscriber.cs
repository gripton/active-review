using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.API.Infrastructure
{
    public class ReviewSessionEventSubscriber: AbstractEventSubscriber, IStartable
    {
        public ReviewSessionEventSubscriber(IDocumentStore store, IReviewSessionMonitor monitor) : base(store, monitor) { }

        public void Start()
        {
            this.Subscribe();
        }
    }
}