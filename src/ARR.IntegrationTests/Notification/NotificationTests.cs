using System.Configuration;

using ARR.Data.Entities;
using ARR.Notifications;

using Xunit;

namespace ARR.IntegrationTests.Notification
{
    public class NotificationTests
    {
        [Fact]
        public void NotificationGenerator_Generates_Succesfully()
        {
            var generator = new NotificationGenerator();

            var args = new NotificationArgs
            {
                BaseUrl = ConfigurationManager.AppSettings["WebUrl"],
                DisplayName = "Matt Schwartz",
                Recipient = "thorfio@gmail.com",
                SessionId = "2434",
                SessionName = "Email Session Test"
            };

            Assert.DoesNotThrow(() => generator.Generate(EventType.ReviewSessionReleased, args));
            Assert.DoesNotThrow(() => generator.Generate(EventType.ReviewerAssigned, args));
            Assert.DoesNotThrow(() => generator.Generate(EventType.QuestionnaireCompleted, args));
        }
    }
}