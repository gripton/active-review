using ARR.AccountManagement;
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
                .AsSelf()
                .InstancePerLifetimeScope();
            
            builder
                .RegisterType<ReviewSessionMonitor>()
                .As<IReviewSessionMonitor>()
                .InstancePerLifetimeScope();

            builder
               .RegisterType<EventRepository>()
               .AsSelf()
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
                .Register(c => new DocumentStore
                {
                    Url = "https://aeo.ravenhq.com/databases/AppHarbor_48e97815-70ea-43bc-ac81-4229e1cc4454",
                    ApiKey = "3d9f210f-2fdc-4eb5-a350-5d4cc3a1e226"
                })
                .As<IDocumentStore>()
                .SingleInstance()
                .OnActivating(c => c.Instance.Initialize());

            builder
                .Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerLifetimeScope()
                .OnActivating(c =>
                    {
                        var test = "test";
                    });               
                
        }
    }

}