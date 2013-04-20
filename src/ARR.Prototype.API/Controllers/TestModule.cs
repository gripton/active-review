using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;

using Raven.Client;
using Raven.Client.Document;

namespace ARR.Prototype.API.Controllers
{
    public class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ReviewSessionManager>()
                .As<IReviewSessionManager>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ReviewSessionRepository>()
                .As<IReviewSessionRepository>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ReviewSessionMonitor>()
                .As<IReviewSessionMonitor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ReviewSessionNotificationGenerator>()
                .As<INotificationGenerator>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<NotificationSender>()
                .As<INotificationSender>()
                .InstancePerLifetimeScope();

            builder
                .Register(c => new DocumentStore { 
                    Url = "https://2.ravenhq.com/databases/AppHarbor_6d9ac094-3ec0-454d-8f3f-b838f0847e99",
                    ApiKey = "8145df81-9552-4979-b44a-7aaf671ddbce"})
                .As<IDocumentStore>()
                .SingleInstance()
                .OnActivating(c => c.Instance.Initialize());

            builder
                .Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
                
                
        }
    }

}