using log4net;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Web.Infrastructure
{
    public interface IWebApplicationServices
    {
        ILog Logger { get; }
        //IAPIClient APIClient { get; }
        IWebSecurityMembershipProvider WebSecurity { get; }   
    }
}
