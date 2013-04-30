using PracticalCode.WebSecurity.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public interface IUserManagementService
    {
        WebSecurityUser GetUser(int Id);
        void CreateUser(WebSecurityUser user);
        void UpdateUser(WebSecurityUser user);
        void UpdateUserProfile(WebSecurityUser user);
        void DeleteUser(WebSecurityUser user);
        bool IsCurrentPasswordValid(string userName, string currentPassword);
    }
}
