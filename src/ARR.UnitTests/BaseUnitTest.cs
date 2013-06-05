using System.Collections.Generic;
using ARR.Data.Entities;

namespace ARR.UnitTests
{
    public class BaseUnitTest
    {
        public Account NewAccount()
        {
            var account = new Account
                {
                    Username = "testUser",
                    EmailAddress = "test@test.com",
                    AreaOfExpertise = "All Of them",
                    ScreenName = "Tester Testington",
                    Password = "1234Test"
                };

            return account;
        }

        public ReviewSession NewReviewSession()
        {
            var session = new ReviewSession();
            session.Title = "Session 1";
            session.Creator = "test@test.com";

            var requirements1 = new Requirement();
            requirements1.Content = "This is requirement 1";

            var requirements2 = new Requirement();
            requirements2.Content = "This is requirement 2";

            var requirements3 = new Requirement();
            requirements3.Content = "This is requirement 3";


            session.Requirements = new List<Requirement> { requirements1, requirements2, requirements3 };

            var question1 = new Question { Content = "This is question 1" };
            var question2 = new Question { Content = "This is question 2" };

            session.Questions = new List<Question> { question1, question2 };

            return session;
        }
    }
}
