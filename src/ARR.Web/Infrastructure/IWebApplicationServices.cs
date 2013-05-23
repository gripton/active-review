using PracticalCode.WebSecurity.Infrastructure.Membership;

namespace ARR.Web.Infrastructure
{
    public interface IWebApplicationServices
    {
        IWebSecurityMembershipProvider WebSecurity { get; }   
    }
}
