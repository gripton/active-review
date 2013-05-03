using System.Web;

using PracticalCode.WebSecurity.Infrastructure.Data;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class CurrentUserHandler : IHttpHandler  
    {     
        public IWebApplicationServices WebApplicationServices { get; set; }
        
        public bool IsReusable
        {
            get { return true; }
        }


        public void ProcessRequest(HttpContext context)
        {
            var user = WebApplicationServices.WebSecurity.GetCurrentUser();
            context.Response.Write(user.JsonSerialize<WebSecurityUser>());
        }
    }
}