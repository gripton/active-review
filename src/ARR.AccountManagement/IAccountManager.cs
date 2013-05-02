using ARR.Data.Entities;
using ARR.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.AccountManagement
{
    public interface IAccountManager
    {
        void CreateNew(Account account);
        void Save(Account account);
        void UpdateSecurityStatistics(Account account);
        void Delete(int Id);

        IReadContext<Account> ReadContext { get; }
    }
}
