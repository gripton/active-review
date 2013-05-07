using ARR.API.Controllers;
using ARR.Data.Entities;
using ARR.Repository;
using Autofac;
using NUnit.Framework;
using TeamAgile.ApplicationBlocks.Interception.UnitTestExtensions;

namespace ARR.IntegrationTests.API
{
    public class ReviewSessionControllerTests : BaseTestController
    {
        [Test, DataRollBack]
        public void Put_Succeeds()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();
            session.Title = "test2";

            var repo = container.Resolve<ReviewSessionRepository>();

            repo.Save(session);

            var updatedSession = repo.GetByName("test2");

            var externallyUpdatedSession = NewReviewSession();
            externallyUpdatedSession.Title = "test2";
            externallyUpdatedSession.SessionStatus = SessionStatusType.Released;
            externallyUpdatedSession.Id = updatedSession.Id;

            var controller = container.Resolve<ReviewSessionController>();

            //IEnumerable<ReviewIndex> sessions;

            Assert.DoesNotThrow(() => { controller.Put(externallyUpdatedSession.Id, string.Empty, externallyUpdatedSession); });
            //Assert.Equals(sessions)
        }
    }
}