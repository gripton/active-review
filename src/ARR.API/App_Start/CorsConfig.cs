using System.Web.Http;
using Thinktecture.IdentityModel.Http.Cors;
using Thinktecture.IdentityModel.Http.Cors.WebApi;

namespace ARR.API.App_Start
{
    public class CorsConfig
    {

        public static void RegisterCors(CorsConfiguration corsConfig)
        {
            corsConfig.AllowAll();
        }

    }
}