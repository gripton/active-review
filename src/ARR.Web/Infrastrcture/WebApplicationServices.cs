using log4net;

using PracticalCode.WebSecurity.Infrastructure.Membership;

namespace ARR.Web.Infrastructure
{
    public class WebApplicationServices : IWebApplicationServices
    {
        private readonly ILog _logger;       
        private readonly IWebSecurityMembershipProvider _webSecurity;
        
        public WebApplicationServices(ILog logger, IWebSecurityMembershipProvider webSecurity) //, IAPIClient apiClient)
        {            
            _logger = logger;
            //_apiClient = apiClient;
            _webSecurity = webSecurity;
        }

        public ILog Logger
        {
            get { return _logger; }
        }       

        public IWebSecurityMembershipProvider WebSecurity
        {
            get { return _webSecurity; }
        }       
    }
}