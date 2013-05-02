using ARR.AccountManagement;
using ARR.Data.Entities;
using ARR.Prototype.API.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ARR.API.Controllers
{
    public class AccountIndexController : ApiController
    {
        private readonly IAccountManager _manager;

        public AccountIndexController(IAccountManager manager)
        {
            _manager = manager;
        }

        // GET api/account
        public IEnumerable<AccountIndex> Get()
        {
            var indexes = new List<AccountIndex>();
            var accounts = _manager.ReadContext.ListAll();

            foreach (var account in accounts)
            {
                indexes.Add(Mapper.Map<AccountIndex>(account));
            }

            return indexes;
        }

        // GET api/account/5
        public AccountIndex Get(int id)
        {
            var account = _manager.ReadContext.Get(id);
            var index = Mapper.Map<AccountIndex>(account);
            return index;
        }

        // POST api/account
        public void Post(AccountIndex account)
        {
            //_manager.CreateNew(account);
        }

        // PUT api/account/5/patch
        public void Put(int id, string patch, AccountIndex account)
        {
            //switch (patch)
            //{
            //    case "security":
            //        _manager.UpdateSecurityStatistics(account);
            //        break;
            //    default:
            //        _manager.Save(account);
            //        break;
            //}
        }

        // DELETE api/account/5
        public void Delete(int id)
        {
            //_manager.Delete(id);
        }
    }
}
