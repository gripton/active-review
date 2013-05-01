using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Raven.Abstractions.Data;
using ARR.Data.Entities;

namespace ARR.Data.Patch
{
    public class AccountPatchCollection
    {
        public static PatchRequest[] GetUpdateStatisticsPatch(Account account)
        {
            return new PatchRequest[] { 

                new PatchRequest { 
                    Type = PatchCommandType.Set, Name = "FailedPasswordAttempt", 
                    Value = account.FailedPasswordAttempt
                },

                new PatchRequest
                {
                    Type = PatchCommandType.Set, Name = "LastModified",
                    Value = account.LastModified
                },

                new PatchRequest
                {
                    Type = PatchCommandType.Set, Name = "LastLogin",
                    Value = account.LastLogin
                },

                new PatchRequest
                {
                    Type = PatchCommandType.Set, Name = "LastLoginAttempted",
                    Value = account.LastLoginAttempted
                },

                new PatchRequest
                {
                    Type = PatchCommandType.Set, Name = "LastPasswordChanged",
                    Value = account.LastPasswordChanged
                }
            };
        }
    }
}
