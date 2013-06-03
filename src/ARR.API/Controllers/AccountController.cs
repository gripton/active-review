using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARR.AccountManagement;
using ARR.Data.Entities;
using AutoMapper;

namespace ARR.API.Controllers
{
    public class AccountController : BaseController
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
        public HttpResponseMessage Post(Account account)
        {
            try
            {
                _manager.CreateNew(account);
                return GetResponse(account.Id.ToNullSafeString());
            }
            catch (Exception e)
            {
                return GetResponse(e);
            }
        }

        // PUT api/account/5/patch
        public HttpResponseMessage Put(int id, string patch, Account account)
        {
            try{
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
                return GetResponse(id.ToNullSafeString());
            }
            catch (Exception e)
            {
                return GetResponse(e);
            }        
        }

        // DELETE api/account/5
        public void Delete(int id)
        {
            _manager.Delete(id);                
        }
    }
}
