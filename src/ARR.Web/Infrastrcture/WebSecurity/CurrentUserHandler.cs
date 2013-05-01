using ARR.Web.Infrastructure;
using ARR.Web.Infrastructure.WebSecurity;
using PracticalCode.WebSecurity.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.Web.Infrastrcture
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