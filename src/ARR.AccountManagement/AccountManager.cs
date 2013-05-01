using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ARR.Data.Entities;
using ARR.Data.Repository;
using ARR.Notifications;
using ARR.Repository;

using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Encryption;
using PracticalCode.WebSecurity.Infrastructure.Membership;

namespace ARR.AccountManagement
{
    public class AccountManager :  IAccountManager
    {
        private readonly INotificationGenerator _generator;
        private readonly INotificationSender _sender;
        private readonly IAccountMonitor _monitor;
        private readonly AccountRepository _repository;
        private readonly IPasswordManager _passwordManager;
        
        public AccountManager(INotificationSender sender, AccountRepository repository,
           IAccountMonitor monitor, IPasswordManager passwordManager)
        {
            _sender = sender;
            _monitor = monitor;
            _repository = repository;
            _passwordManager = passwordManager;

            ReadContext = repository;
        }

        public IReadContext<Account> ReadContext { get; private set; }

        public void CreateNew(Account account)
        {
            WebSecurityUser user = new WebSecurityUser();
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
    }
}