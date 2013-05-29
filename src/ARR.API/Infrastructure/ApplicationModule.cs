using System.Configuration;
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
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AccountManager>()
                .As<IAccountManager>()
                .InstancePerApiRequest();

            builder
                .RegisterType<AccountRepository>()
                .As<AbstractRepository<Account>>()
                .InstancePerApiRequest();

            builder
                .RegisterType<PasswordManager>()
                .As<IPasswordManager>()
                .InstancePerApiRequest();

            builder
                .RegisterType<ReviewSessionManager>()
                .As<IReviewSessionManager>()
                .InstancePerApiRequest();

            builder
                .RegisterType<ReviewSessionRepository>()
                .As<AbstractRepository<ReviewSession>>()
                .InstancePerApiRequest();
            
            builder
                .RegisterType<ReviewSessionMonitor>()
                .As<IReviewSessionMonitor>()
                .InstancePerApiRequest();

            builder
                .RegisterType<NotificationGenerator>()
                .As<INotificationGenerator>()
                .InstancePerApiRequest();

            builder
                .RegisterType<NotificationSender>()
                .As<INotificationSender>()
                .InstancePerApiRequest();

            builder
                .RegisterType<NotificationSender>()
                .As<INotificationSender>()
                .InstancePerApiRequest();

            builder
               .RegisterType<EventRepository>()
                .As<AbstractRepository<Event>>()
               .InstancePerApiRequest();

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
                .Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerApiRequest();               
                
        }
    }

}