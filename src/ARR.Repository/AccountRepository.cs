using System.Linq;

using ARR.Data.Entities;
using Raven.Client;
using ARR.Data.Patch;

namespace ARR.Repository
{
    public class AccountRepository : AbstractRepository<Account>
    {
        public AccountRepository(IDocumentSession session) : base(session) { }

        public AccountRepository(IDocumentStore store) : base(store) { }

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
