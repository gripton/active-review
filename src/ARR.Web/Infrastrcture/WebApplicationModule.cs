﻿using System.Web;
using ARR.Web.Infrastructure;
using ARR.Web.Infrastructure.WebSecurity;
using Autofac;
using Autofac.Integration.Web;
using log4net;
using PracticalCode.WebSecurity.Infrastructure.Membership;


namespace ARR.Web.Infrastructure
{
    public class WebApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Common Stuff
            builder
              .RegisterInstance(LogManager.GetLogger(typeof(HttpApplication)))
              .As<ILog>()
              .SingleInstance();

            builder
               .RegisterType<WebSecurityMembershipProvider>()
               .As<IWebSecurityMembershipProvider>()
               .InstancePerHttpRequest();

            builder
              .Register(c => new LoginManager(c.Resolve<IWebSecurityMembershipProvider>().DataProvider,
                  c.Resolve<IWebSecurityMembershipProvider>().PasswordManager,
                  c.Resolve<IWebSecurityMembershipProvider>().AuthenticationManager))
              .As<ILoginManager>()
              .InstancePerHttpRequest();            

            builder
                .RegisterType(typeof(WebApplicationServices))
                .As<IWebApplicationServices>()
                .InstancePerHttpRequest();
            
            builder.RegisterModule<WebSecurityModule>();
        }
    }
}