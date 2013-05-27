using System.Web.Http;
using Thinktecture.IdentityModel.Http.Cors.WebApi;

namespace ARR.API.App_Start
{
    public class CorsConfig
    {

        public static void RegisterCors(HttpConfiguration httpConfig)
        {
            var corsConfig = new WebApiCorsConfiguration();
            corsConfig.RegisterGlobal(httpConfig);

            corsConfig.ForAllOrigins().AllowAll();
        }

    }
}