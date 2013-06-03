using ARR.Background;
using ARR.Background.Events;

using Autofac;

using Xunit;

namespace ARR.IntegrationTests.Background
{
    public class SchedulerResolutionTests
    {
        [Fact]
        public void Scheduler_Resolves_Successfully()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<BackgroundModule>();
            var container = builder.Build();

            Assert.DoesNotThrow(() => container.Resolve<IEventPublisherScheduler>());
        } 
    }
}