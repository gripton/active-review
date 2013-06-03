using System;
using System.Configuration;

using ARR.Background.Events;
using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;
using ARR.ReviewSessionManagement;

using Autofac;

using Raven.Client;
using Raven.Client.Document;

namespace ARR.Background
{
    public class BackgroundModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder
                .RegisterType<NotificationGenerator>()
                .As<INotificationGenerator>()
                .SingleInstance();

            builder
                .RegisterType<NotificationSender>()
                .As<INotificationSender>()
                .SingleInstance();

            builder
                .Register(c => new DocumentStore
                    {
                        Url = ConfigurationManager.AppSettings["RavenUrl"],
                        ApiKey = ConfigurationManager.AppSettings["RavenKey"]
                    })
                .As<IDocumentStore>()
                .SingleInstance()
                .OnActivating(c => c.Instance.Initialize());


            builder
                .Register(c => new AccountRepository(c.Resolve<IDocumentStore>()))
                .As<AbstractRepository<Account>>()
                .SingleInstance();

            builder
                .Register(c => new ReviewSessionRepository(c.Resolve<IDocumentStore>()))
                .As<AbstractRepository<ReviewSession>>()
                .SingleInstance();


            builder
                .Register(c => new EventRepository(c.Resolve<IDocumentStore>()))
                .As<AbstractRepository<Event>>()
                .SingleInstance();

            builder
                .RegisterType<ReviewSessionMonitor>()
                .As<IObserver<Event>>()
                .SingleInstance();

            builder
                .RegisterType<EventPublisherJobFactory>()
                .AsSelf()
                .SingleInstance();

           builder
               .Register(GetEventPublishScheduler)
               .As<IEventPublisherScheduler>()
               .SingleInstance();
                
        }

        private static IEventPublisherScheduler GetEventPublishScheduler(IComponentContext c)
        {
            var jobFactory = c.Resolve<EventPublisherJobFactory>();
            var resolver = new EventPublisherResolver(jobFactory);
            var manager = new EventPublisherScheduler(resolver);

            return manager;
        }
    }

}