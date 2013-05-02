using System;
using System.Collections.Generic;
using System.Linq;

using ARR.Data.Entities;
using Raven.Abstractions.Data;
using Raven.Client;
using ARR.Data.Patch;

namespace ARR.Repository
{
    public class AccountRepository : AbstractRepository<Account>
    {
        public AccountRepository(IDocumentSession session)
            : base(session)
        {
            
        }

        public override Account GetByName(string name)
        {
            return Find((a) => a.Username == name).FirstOrDefault();
        }

        protected override void InitializePatchFunctions()
        {
            PatchDictionary.Add(Account.UpdateSecurityPatch, AccountPatchCollection.GetUpdateStatisticsPatch);
        }
    }
    
}
