using System;
using System.IO;
using System.Web;

using ARR.Data.Entities;

using PracticalCode.WebSecurity.Infrastructure.Data;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using PracticalCode.WebSecurity.Infrastructure.Policies;

namespace ARR.Web.Infrastructure.WebSecurity
{
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

                LoginStatus status;

                try
                {
                    status = LoginManager.DefaultLogin(account.Username, account.Password,
                        WebSecuritySettings, context.Request.QueryString);                    
                }
                catch (Exception ex)
                {
                    WebApplicationServices.Logger.Error(_exceptionMessage, ex);
                    status = LoginStatus.UnAuthorized;
                }

                SetResponse(context, status);

                // TODO: Add this functionality to javascript
                //if (status == LoginStatus.PasswordExpired)
                //{
                //    return RedirectToAction("ChangePassword", "User");
                //}

                // Do if any Un-Authorized, Locked-out, or Invalid Password
                //if (status != LoginStatus.Authenticated)
                //{
                //    return View(model);
                //}

                // Do if we're good
                //if (!returnUrl.IsNullOrEmpty())
                //{
                //    return Redirect(returnUrl);
                //}

                //return RedirectToAction("Index", "Home");
            }
        }

    }
}