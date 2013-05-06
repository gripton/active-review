using ARR.AccountManagement;
using ARR.API.Infrastructure;
using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using Autofac.Integration.WebApi;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using Raven.Client;
using Raven.Client.Document;

namespace ARR.API.Controllers
{
    public class BackgroundModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AccountEventSubscriber>()
                .AsSelf()
                .As<IStartable>()
                .SingleInstance();

            builder
                .RegisterType<ReviewSessionEventSubscriber>()
                .As<IStartable>()
                .SingleInstance();  

           builder
                .RegisterType<NotificationGenerator>()
                .As<INotificationGenerator>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<NotificationSender>()
                .As<INotificationSender>()
                .InstancePerLifetimeScope();     

            builder
                .Register(c => new DocumentStore
                {
                    Url = "https://2.ravenhq.com/databases/AppHarbor_6d9ac094-3ec0-454d-8f3f-b838f0847e99",
                    ApiKey = "8145df81-9552-4979-b44a-7aaf671ddbce"
                })
                .As<IDocumentStore>()
                .SingleInstance()
                .OnActivating(c => c.Instance.Initialize());


            builder
                .Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<AccountRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ReviewSessionRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<AccountMonitor>()
                .As<IAccountMonitor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ReviewSessionMonitor>()
                .As<IReviewSessionMonitor>();    
                
        }
    }

}