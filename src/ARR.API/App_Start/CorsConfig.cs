using Thinktecture.IdentityModel.Http.Cors;

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