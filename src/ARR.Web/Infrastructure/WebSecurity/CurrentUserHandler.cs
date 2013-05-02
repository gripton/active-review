using System.Web;

using PracticalCode.WebSecurity.Infrastructure.Data;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class CurrentUserHandler : JsonRequestHandler    {     

        public IWebApplicationServices WebApplicationServices { get; set; }
        
        public override void ProcessRequest(HttpContext context)
        {
            var user = WebApplicationServices.WebSecurity.GetCurrentUser();
            SetResponse<WebSecurityUser>(context, user);            
        }

    }
}