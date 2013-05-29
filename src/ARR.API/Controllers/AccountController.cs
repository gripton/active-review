using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARR.AccountManagement;
using ARR.Data.Entities;

namespace ARR.API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountManager _manager;

        public AccountController(IAccountManager manager)
        {
            _manager = manager;
        }

        // GET api/account
        public IEnumerable<Account> Get()
        {
            return _manager.ReadContext.ListAll();
        }

        // GET api/account/5
        public Account Get(string id)
        {
            return _manager.ReadContext.GetByName(id);
        }

        // POST api/account
        public void Post(Account account)
        {
            _manager.CreateNew(account);
        }

        // PUT api/account/5/patch
        public void Put(int id, string patch, Account account)
        {
            switch (patch)
            {
                case "security":
                    _manager.UpdateSecurityStatistics(account);
                    break;
                case "update-profile":
                    _manager.UpdateSecurityStatistics(account);
                    break;
                default:
                    _manager.Save(account);
                    break;
            }
        }

        // DELETE api/account/5
        public void Delete(int id)
        {
            _manager.Delete(id);                
        }
    }
}
