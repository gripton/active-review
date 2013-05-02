using log4net;
using PracticalCode.WebSecurity.Infrastructure.Membership;

namespace ARR.Web.Infrastructure
{
    public interface IWebApplicationServices
    {
        ILog Logger { get; }
        IWebSecurityMembershipProvider WebSecurity { get; }   
    }
}
