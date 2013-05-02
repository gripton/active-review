using Autofac;
using Autofac.Integration.Web;

using PracticalCode.WebSecurity.Infrastructure.Policies;
using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Authorization;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class WebSecurityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterPolicies(builder);
            RegisterServices(builder);
            base.Load(builder);
        }

        static void RegisterPolicies(ContainerBuilder builder)
        {
            builder
                .Register(c => new PasswordPolicySettings {
                    AttemptWindow = 30,
                    AuthenticationTimeout = 60,
                    ExpirationDays = 120,
                    InvalidPasswordErrorMessage = "Invalid Password",
                    MaxInvalidAttempts = 5,
                    MinRequiredLength = 6
                })
                .As<IWebSecurityPolicySettings>()
                .SingleInstance();
        }

        static void RegisterServices(ContainerBuilder builder)
        {
            builder
               .RegisterType<RoleProvider>()
               .As<IWebSecurityRoleProvider>()
               .InstancePerHttpRequest();

            builder
               .RegisterType<WebSecurityDataProvider>()
               .As<IWebSecurityDataProvider>()
               .InstancePerHttpRequest();

            builder
                .RegisterType(typeof(UserManagementService))
                .As<IUserManagementService>()
                .InstancePerHttpRequest();           
        }
    }

    public class RoleProvider : IWebSecurityRoleProvider
    {
        public WebSecurityUser GetCurrentUser()
        {
            throw new System.NotImplementedException();
        }

        public bool IsUserInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }
    }
}