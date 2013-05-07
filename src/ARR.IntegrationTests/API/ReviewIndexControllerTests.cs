using System.Collections.Generic;
using ARR.API.Controllers;
using ARR.API.Models;
using ARR.Data.Entities;
using ARR.Repository;
using Autofac;
using NUnit.Framework;
using TeamAgile.ApplicationBlocks.Interception.UnitTestExtensions;

namespace ARR.IntegrationTests.API
{
    [TestFixture]
    public class ReviewIndexControllerTests : BaseTestController
    {
        [Test, DataRollBack]
        public void Get_Succeeds()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();

            var repo = container.Resolve<ReviewSessionRepository>();

            repo.Save(session);

            var controller = container.Resolve<ReviewIndexController>();

            IEnumerable<ReviewIndex> sessions;

            Assert.DoesNotThrow(() => { sessions = controller.Get(); });
            //Assert.Equals(sessions)
        }

        private ReviewSession NewReviewSession()
        {
             var session = new ReviewSession();
            session.Title = "Session 1";

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
