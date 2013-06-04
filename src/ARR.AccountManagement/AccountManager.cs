
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using ARR.AccountManagement.Exceptions;
using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;

using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Encryption;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using PracticalCode.WebSecurity.Infrastructure.Policies.Exceptions;

namespace ARR.AccountManagement
{
    public class AccountManager :  IAccountManager
    {
        //private readonly INotificationGenerator _generator;
        private readonly AbstractRepository<Account> _repository;
        private readonly IPasswordManager _passwordManager;
        
        public AccountManager(AbstractRepository<Account> repository, IPasswordManager passwordManager)
        {
            _repository = repository;
            _passwordManager = passwordManager;

            ReadContext = repository;
        }

        public IReadContext<Account> ReadContext { get; private set; }

        public void CreateNew(Account account)
        {
            if(UserExists(account.Username))
                throw new UserAlreadyExistsException();

            if(InvalidUserName(account.Username))
                throw new InvalidUsernameException();

            if (InvalidEmailAddres(account.EmailAddress))
                throw new InvalidEmailAddressException();

            var user = new WebSecurityUser();
            user.SetDefaultStatistics("system");
            
            // Copy over default stats
            account.LastModified = user.Statistics.LastModified.Value;
            account.LastLogin = user.Statistics.LastLogin.Value;
            account.LastLoginAttempted = user.Statistics.LastLoginAttempted.Value;
            account.LastPasswordChanged = user.Statistics.LastPasswordChanged.Value;
            
            account.Password = 
                _passwordManager.EncryptPassword(account.Password, BCryptEncoder.GenerateSalt(), BCryptEncoder.HashPassword);
            
            _repository.Save(account);
        }

        public void Save(Account account)
        {
            _repository.Save(account);
        }

        public void UpdateSecurityStatistics(Account account)
        {
            _repository.Patch(account, Account.UpdateSecurityPatch);
        }
        
        public void Delete(int id)
        {
            var account = _repository.Get(id);
            _repository.Delete(account);
        }

        private static bool InvalidUserName(string username)
        {
            return !Regex.IsMatch(username, "^[a-zA-Z0-9_]*$");
        }

        private static bool InvalidEmailAddres(string emailaddress)
        {
            try
            {
                var m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool UserExists(string username)
        {
            return _repository.GetByName(username) != null;
        }
    }
}