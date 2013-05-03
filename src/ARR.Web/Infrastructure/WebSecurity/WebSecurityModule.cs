using Autofac;
using Autofac.Integration.Web;

using PracticalCode.WebSecurity.Infrastructure.Policies;
using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Authorization;
using PracticalCode.WebSecurity.Infrastructure.Authentication;
using System.Web;
using System.Web.Security;
using System.Text;
using System;
using Newtonsoft.Json;

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
               .RegisterType<MyRoleProvider>()
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

    public class MyRoleProvider : RoleProvider, IWebSecurityRoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            var user = GetCurrentUser();
            return (user.HasRoles) ? user.SecurityRoles : new string[0];
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = GetCurrentUser();
            return true;// TODO ((user.HasRoles) && (user.SecurityRoles.Contains(roleName)));
        }

        public bool IsUserInRole(string roleName)
        {
            return IsUserInRole(string.Empty, roleName);
        }

        /// <summary>
        /// Populates an Web Security User Object by the data sotred in the FormsAuthentication.UserData field.
        /// </summary>
        /// <returns></returns>
        public WebSecurityUser GetCurrentUser()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var id = (FormsIdentity)HttpContext.Current.User.Identity;
                var ticket = id.Ticket;

                // Get the stored user-data, in this case, our roles
                var user = ticket.UserData.JsonDeserialize<WebSecurityUser>();
                return user;
            }

            // Method should not have reached this point. If so throw an UnAuthenticatedAccessException.
            var msg = new StringBuilder("Attemp to populate the web security user object by the user data in the Forms Authentication Ticket.UserData field failed.");
            msg.Append("This failure has occurred because there is no Current User stored in the HTTPContext.Current object which should have occured after authentication.");
            msg.Append("Please make sure that this method has been called only after Authentication has been successfully performed on the Current User.");

            throw new UnAuthenticatedAccessException(msg.ToString());

        }

        #region Not yet implemented

        public override string ApplicationName
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override string[] GetAllRoles() { throw new NotSupportedException(); }
        public override void CreateRole(string roleName) { throw new NotSupportedException(); }
        public override bool RoleExists(string roleName) { throw new NotSupportedException(); }
        public override string[] GetUsersInRole(string roleName) { throw new NotSupportedException(); }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) { throw new NotSupportedException(); }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}