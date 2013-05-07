using System.Collections.Generic;
using ARR.API.Controllers;
using ARR.API.Models;
using ARR.Data.Entities;
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

            // Register the Web API controllers.
            builder
                .RegisterType<ReviewSessionController>()
                .AsSelf();

            // Build the container.
            var container = builder.Build();

            return container;
        }

        protected ReviewSession NewReviewSession()
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