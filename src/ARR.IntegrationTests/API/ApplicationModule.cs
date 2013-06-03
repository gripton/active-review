using ARR.AccountManagement;
using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using Raven.Client;
using Raven.Client.Document;

namespace ARR.IntegrationTests.API
{
    public class TestApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AccountManager>()
                .As<IAccountManager>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<AccountRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<PasswordManager>()
                .As<IPasswordManager>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ReviewSessionManager>()
                .As<IReviewSessionManager>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ReviewSessionRepository>()
                .As<AbstractRepository<ReviewSession>>()
                .InstancePerLifetimeScope();
            
            builder
                .RegisterType<ReviewSessionMonitor>()
                .As<IReviewSessionMonitor>()
                .InstancePerLifetimeScope();

            builder
               .RegisterType<EventRepository>()
                .As<AbstractRepository<Event>>()
               .InstancePerLifetimeScope();

            builder
                .RegisterType<NotificationGenerator>()
                .As<INotificationGenerator>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<NotificationSender>()
                .As<INotificationSender>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<NotificationSender>()
                .As<INotificationSender>()
                .InstancePerLifetimeScope();

            builder
                .Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope();
                
        }
    }

}