using System.Collections.Generic;

using PracticalCode.WebSecurity.Infrastructure.Membership;
using PracticalCode.WebSecurity.Infrastructure.Policies;
using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Encryption;

namespace ARR.Web.Infrastructure.WebSecurity
{
    /// <summary>
    /// A facade for integrating with Web Security on user management.
    /// </summary>
    public class UserManagementService : IUserManagementService
	{
        private readonly IWebApplicationServices _applicationServices;
        private readonly IPasswordManager _passwordManager;
        private readonly IWebSecurityDataProvider _dataProvider;
        private readonly IWebSecurityPolicyEnforcer _policyEnforcer;


        public UserManagementService( IWebApplicationServices applicationServices)
        {
            _applicationServices = applicationServices;

            _dataProvider = applicationServices.WebSecurity.DataProvider;
            _passwordManager = applicationServices.WebSecurity.PasswordManager;
            _policyEnforcer = applicationServices.WebSecurity.PolicyEnforcer;
        }

        public WebSecurityUser Authorizer { get { return _applicationServices.WebSecurity.GetCurrentUser(); } }

        public void CreateUser(WebSecurityUser user)
        {
            var assertions = new List<PolicyAssertion>
                                                   {
                                                       new PasswordLengthPolicy(),
                                                       new PasswordStrengthPolicy(),
                                                       new PasswordCharactersPolicy()
                                                   };

            _policyEnforcer.EnforceUniqueUser(user);
            _policyEnforcer.EnforcePasswordPolicies(user, assertions);

            user.Username = user.NewUsername;
            user.Password = _passwordManager.EncryptPassword(user.NewPassword, BCryptEncoder.GenerateSalt(), BCryptEncoder.HashPassword);

            user.SetDefaultStatistics(Authorizer.Username);

            _dataProvider.CreateUser(user);
        }

        public void UpdateUser(WebSecurityUser user)
        {
           UpdateUser(user, Authorizer.Username);
        }

        public void UpdateUserProfile(WebSecurityUser user)
        {
            UpdateUser(user, user.Username);
        }

        public void DeleteUser(WebSecurityUser user)
        {
            _dataProvider.DeleteUser(user);
        }

        private void UpdateUser(WebSecurityUser user, string authorizer)
        {
            //_policyEnforcer.EnforceUniqueUser(user);

            //if (!user.NewPassword.IsNullOrEmpty())
            //{
            //    var password =
            //        from pUser in _applicationServices.ReadContext.Users.Find(u => u.Username == user.Username)
            //        select pUser.Password;

            //    user.Password = password.SingleOrDefault();

            //    _policyEnforcer.EnforcePasswordPolicies(user);
            //    user.NewPassword = _passwordManager.EncryptPassword(user.NewPassword, BCryptEncoder.GenerateSalt(),
            //                                                     BCryptEncoder.HashPassword);

            //    user.Statistics.LastPasswordChanged = DateTime.UtcNow;
            //}

            //user.Statistics.LastModified = DateTime.UtcNow;
            //user.Statistics.LastModifiedBy = authorizer;

            //_dataProvider.UpdateUser(user);
        }

        public WebSecurityUser GetUser(int Id)
        {
            //var user = _applicationServices.ReadContext.Users.Get(Id);

            //if (user == null) return null;

            //var webuser = Mapper.Map<User, WebSecurityUser>(user);
            return new WebSecurityUser();
        }

        public bool IsCurrentPasswordValid(string userName, string currentPassword)
        {
            //var password =
            //        from pUser in _applicationServices.ReadContext.Users.Find(u => u.Username == userName)
            //        select pUser.Password;

            //var encryptedCurrentPassword = BCryptEncoder.CheckPassword(currentPassword, password.SingleOrDefault());
            //    //_passwordManager.EncryptPassword(currentPassword, 
            //      //                          BCryptEncoder.GenerateSalt(), BCryptEncoder.HashPassword);

            //return encryptedCurrentPassword;
            return false;
        }


	}
}

