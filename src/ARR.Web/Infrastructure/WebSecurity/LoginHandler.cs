using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Management;
using ARR.Data.Entities;

using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using PracticalCode.WebSecurity.Infrastructure.Policies;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class LoginErrorEvent : WebRequestErrorEvent
    {
        public LoginErrorEvent(string message, Exception ex)
            : base(message, null, 100001, ex)
        {
        }

    }

    public class LoginHandler : JsonRequestHandler
    {
        private const string _anonymousMessage = @"Anonymous user failed to login with a username of {0}";
        private const string _invalidUserMessage = @"The username or password provided is incorrect.";
        private const string _lockedUserMessage = @"The account is locked out. Please contact support to unlock.";
        private const string _exceptionMessage = @"Unable to log on. An unexpected error occurred.";

        public IWebApplicationServices WebApplicationServices { get; set; }
        public ILoginManager LoginManager { get; set; }
        public IWebSecurityPolicySettings WebSecuritySettings { get; set; }

        public override void ProcessRequest(HttpContext context)
        {
            context.Request.InputStream.Position = 0;
            using (StreamReader inputStream = new StreamReader(context.Request.InputStream))
            {
                var json = inputStream.ReadToEnd();
                var account = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(json);

                LoginResult result = new LoginResult();

                try
                {
                    result.Status = LoginManager.DefaultLogin(account.Username.Trim(), account.Password.Trim(),
                        WebSecuritySettings, context.Request.QueryString);                    
                }
                catch (Exception ex)
                {
                    const string format = "Login Failed with APIUrl '{0}'";

                    var errorEvent = 
                        new LoginErrorEvent(string.Format(format, ConfigurationManager.AppSettings["ApiUrl"]), ex);

                    errorEvent.Raise();

                    result.Status = LoginStatus.UnAuthorized;
                }

                SetResponse(context, result);
            }
        }


        private class LoginResult
        {
            public LoginStatus Status { get; set; }
        }
    }
}