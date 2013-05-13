using System.Collections.Generic;

using ARR.API.Controllers;
using ARR.API.Models;
using ARR.Repository;

using Autofac;
using Xunit;

namespace ARR.IntegrationTests.API
{
    public class ReviewIndexControllerTests : BaseIntegrationTest
    {
        [Fact]
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
    }
}
