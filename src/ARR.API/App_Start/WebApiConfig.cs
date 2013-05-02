using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ARR.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "PatchApi",
                routeTemplate: "api/{controller}/{id}/{patch}",
                defaults: new { id = RouteParameter.Optional, patch = RouteParameter.Optional }
            );
        }
    }
}
