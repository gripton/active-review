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
                
        }
    }

}