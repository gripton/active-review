using System.Web;
using ARR.Data.Entities;

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
            var account = AutoMapper.Mapper.Map<Account>(user);
            context.Response.Write(account.JsonSerialize());
        }
    }
}