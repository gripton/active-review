using System;
namespace ARR.Data.Entities
{
    public class Account : IPersistentEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ScreenName { get; set; }
        public string Organization { get; set; }
        public string AreaOfExpertise { get; set; }

        // Web Secutiry Stats
        public DateTime LastModified { get; set; }        
        public DateTime LastLogin { get; set; }
        public DateTime LastLoginAttempted { get; set; }
        public DateTime LastPasswordChanged { get; set; }
        public int FailedPasswordAttempt { get; set; }
    }
}
