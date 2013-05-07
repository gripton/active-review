using ARR.API.Controllers;
using ARR.API.Models;
using Autofac;

namespace ARR.IntegrationTests.API
{
    public class BaseTestController
    {
        protected IContainer Setup()
        {
            // Map AutoMapper
            AutoMapper.Mapper.AddProfile<IndexMappingProfile>();

            // Create the container builder.
            var builder = new ContainerBuilder();

            builder.RegisterModule(new TestApplicationModule());

            // Register the Web API controllers.
            builder
                .RegisterType<ReviewIndexController>()
                .AsSelf();

            // Build the container.
            var container = builder.Build();

            return container;
        }
    }
}