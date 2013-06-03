using System.Configuration;
using ARR.AccountManagement;
using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using Autofac.Integration.WebApi;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using Raven.Client;
using Raven.Client.Document;

namespace ARR.API.Infrastructure
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
                .Register(c => new AccountRepository(c.Resolve<IDocumentSession>()))
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
                .Register(c => new ReviewSessionRepository(c.Resolve<IDocumentSession>()))
                .As<AbstractRepository<ReviewSession>>()
                .InstancePerApiRequest();
            
            builder
               .Register(c => new ReviewSessionRepository(c.Resolve<IDocumentSession>()))
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