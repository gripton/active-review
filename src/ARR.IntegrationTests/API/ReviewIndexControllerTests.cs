using ARR.API.Controllers;

using Autofac;
using NUnit.Framework;

namespace ARR.IntegrationTests.API
{
    [TestFixture]
    public class ReviewIndexControllerTests : BaseTestController
    {
        [Test]
        public void Get_Succeeds()
        {
            // Build the container.
            var container = Setup();

            var controller = container.Resolve<ReviewIndexController>();

            Assert.DoesNotThrow(() => controller.Get());
        }
    }
}
