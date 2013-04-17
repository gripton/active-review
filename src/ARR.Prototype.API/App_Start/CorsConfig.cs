using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.Http.Cors.WebApi;

namespace ARR.Prototype.API
{
    public class CorsConfig
    {

        public static void RegisterCors(HttpConfiguration httpConfig)
        {

            WebApiCorsConfiguration corsConfig = new WebApiCorsConfiguration();
            corsConfig.RegisterGlobal(httpConfig);

            corsConfig.AllowAll();

        }

    }
}