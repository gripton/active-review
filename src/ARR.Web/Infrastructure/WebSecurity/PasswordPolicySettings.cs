using PracticalCode.WebSecurity.Infrastructure.Policies;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class PasswordPolicySettings : IWebSecurityPolicySettings
    {
        public int HistoryCount { get; set; }       
        public int MaxInvalidAttempts { get; set; }
        public int MinRequiredNonAlphanumericCharacters { get; set; }
        public int MinRequiredLength { get; set; }
        public int AttemptWindow { get; set; }
        public string StrengthRegularExpression { get; set; }
        public int ExpirationDays { get; set; }
        public string InvalidPasswordErrorMessage { get; set; }
        public int AuthenticationTimeout { get; set; }
    }
}
