
using System.Collections.Generic;
using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using Xunit;
using AutoMapper;

namespace ARR.IntegrationTests.ReviewManagement
{
    public class ReviewSessionQuestionnaireTests : BaseIntegrationTest
    {
        [Fact]
        public void SaveQuestionnaire_Succeeds()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                // Create a new session to work with
                var session = NewReviewSession();
                sessionRepo.Save(session);

                AutoMapper.Mapper.CreateMap<Question, Question>();
                var questions = new List<Question>();

                foreach (var question in session.Questions)
                {
                    questions.Add(Mapper.Map<Question>(question));
                }

                questions[0].Content = "test was changed";

                Assert.DoesNotThrow(() => manager.SaveQuestionnaire(session.Id, questions, session.Creator));
            }
        }
    }
}
