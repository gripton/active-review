
using System.Collections.Generic;
using System.Linq;
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

            // Create a new session to work with
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Released;
            session.Reviewer = "test@test.com";

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                sessionRepo.Save(session);
            }


            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Mapper.CreateMap<Question, Question>();

                var questions = session.Questions.Select(Mapper.Map<Question>).ToList();

                questions[0].Content = "test was changed";

                Assert.DoesNotThrow(() => manager.SaveQuestionnaire(session.Id, questions, session.Reviewer));
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var savedSession = sessionRepo.Get(session.Id);

                Assert.Equal(2, savedSession.Questions.Count);
                Assert.Equal("test was changed", savedSession.Questions[0].Content);
            }
        }
    }
}
