using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARR.Data.Entities;
using Raven.Client;
using ARR.Data.Patch;

namespace ARR.Data.Repository
{
    public class AccountRepository : AbstractRepository<Account>
    {
        public AccountRepository(IDocumentSession session, IAccountPatcher patcher)
            : base(session)
        {
            Patcher = patcher;
        }

        public IAccountPatcher Patcher { get; set; }

        public override Account GetByName(string name)
        {
            return Find((a) => a.Username == name).FirstOrDefault();
        }
    }
    
}
