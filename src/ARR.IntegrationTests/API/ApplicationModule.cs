using ARR.AccountManagement;
using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using Raven.Client;

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
                .Register(c => new AccountRepository(c.Resolve<IDocumentSession>()))
                .As<AbstractRepository<Account>>()
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
                .Register(c => new ReviewSessionRepository(c.Resolve<IDocumentSession>()))
                .As<AbstractRepository<ReviewSession>>()
                .InstancePerLifetimeScope();

            builder
               .Register(c => new EventRepository(c.Resolve<IDocumentSession>()))
                .As<AbstractRepository<Event>>()
               .InstancePerLifetimeScope();
            
            builder
                .RegisterType<ReviewSessionMonitor>()
                .As<IReviewSessionMonitor>()
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