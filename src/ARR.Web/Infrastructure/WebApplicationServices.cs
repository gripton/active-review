using PracticalCode.WebSecurity.Infrastructure.Membership;


namespace ARR.Web.Infrastructure
{
    public class WebApplicationServices : IWebApplicationServices{
        
        private readonly IWebSecurityMembershipProvider _webSecurity;
        
        public WebApplicationServices(IWebSecurityMembershipProvider webSecurity)
        {  
            _webSecurity = webSecurity;
        }
        
        public IWebSecurityMembershipProvider WebSecurity
        {
            get { return _webSecurity; }
        }       
    }
}