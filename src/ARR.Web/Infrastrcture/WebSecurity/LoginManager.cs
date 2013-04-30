using PracticalCode.WebSecurity.Infrastructure.Authentication;
using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Membership;

using System.Collections.Specialized;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class LoginManager : AbstractLoginManager
    {
        public LoginManager(IWebSecurityDataProvider dataProvider, IPasswordManager passwordManager,
            IAuthenticationManager authenticationManager) : base(dataProvider, passwordManager, authenticationManager)
        {

        }

        public override void LoadUserInfo(WebSecurityUser user, NameValueCollection query)
        {
            
        }
    }
}