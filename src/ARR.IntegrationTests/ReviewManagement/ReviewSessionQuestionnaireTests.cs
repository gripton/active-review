
using System;
using System.Collections.Generic;
using System.Linq;
using ARR.Core.Authorization;
using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using ARR.ReviewSessionManagement.Exceptions;
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

                Assert.Equal(SessionStatusType.Released, savedSession.SessionStatus);
                Assert.Equal(2, savedSession.Questions.Count);
                Assert.Equal("test was changed", savedSession.Questions[0].Content);
            }
        }

        [Fact]
        public void SaveQuestionnaire_Fails_Not_Found()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<SessionNotFoundException>(() => manager.SaveQuestionnaire(1, new List<Question>(), "test"));
            }
        }

        [Fact]
        public void SaveQuestionnaire_Fails_UnAuthorized()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Released;

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();

                // Create a new session to work with
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);
                
                Assert.Throws<AuthorizationException>(() => manager.SaveQuestionnaire(session.Id, new List<Question>(), "FAIL"));
            }
        }

        [Fact]
        public void SaveQuestionnaire_Fails_Invalid_Status()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Completed;

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();

                // Create a new session to work with
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<InvalidOperationException>(() => manager.SaveQuestionnaire(session.Id, new List<Question>(), session.Reviewer));
            }
        }

        [Fact]
        public void CompleteQuestionnaire_Succeeds()
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

                Assert.DoesNotThrow(() => manager.CompleteQuestionnaire(session.Id, questions, session.Reviewer));
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var savedSession = sessionRepo.Get(session.Id);

                Assert.Equal(SessionStatusType.Completed, savedSession.SessionStatus);
                Assert.Equal(2, savedSession.Questions.Count);
                Assert.Equal("test was changed", savedSession.Questions[0].Content);

                //TODO: Figure out why the build server test break on Event.EventType assertion
                //var events = eventRepo.ListAll();

                //Assert.Equal(1, events.Count);
                //Assert.Equal(events[0].EventType, EventType.QuestionnaireCompleted);
                //Assert.Equal(events[0].EntityId, session.Id);
            }
        }

        [Fact]
        public void CompleteQuestionnaire_Fails_Not_Found()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<SessionNotFoundException>(() => manager.CompleteQuestionnaire(1, new List<Question>(), "test"));
            }
        }

        [Fact]
        public void CompleteQuestionnaire_Fails_UnAuthorized()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Released;

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();

                // Create a new session to work with
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<AuthorizationException>(() => manager.CompleteQuestionnaire(session.Id, new List<Question>(), "FAIL"));
            }
        }

        [Fact]
        public void CompleteQuestionnaire_Fails_Invalid_Status()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Completed;

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();

                // Create a new session to work with
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<InvalidOperationException>(() => manager.CompleteQuestionnaire(session.Id, new List<Question>(), session.Reviewer));
            }
        }

        [Fact]
        public void ProvideFeedback_Succeeds()
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

                questions[0].Feedbacks = new List<Feedback>
                    {
                        new Feedback
                            {
                                Created = DateTime.UtcNow,
                                Text = "This is new feedback",
                                Username = session.Reviewer
                            }
                    };

                Assert.DoesNotThrow(() => manager.ProvideFeedback(session.Id, questions, session.Reviewer));
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var savedSession = sessionRepo.Get(session.Id);

                Assert.Equal(SessionStatusType.Released, savedSession.SessionStatus);
                Assert.Equal(1, savedSession.Questions[0].Feedbacks.Count);
                Assert.Equal("This is new feedback", savedSession.Questions[0].Feedbacks[0].Text);
                Assert.Equal(session.Reviewer, savedSession.Questions[0].Feedbacks[0].Username);
            }
        }

        [Fact]
        public void ProvideFeedback_Fails_Not_Found()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<SessionNotFoundException>(() => manager.ProvideFeedback(1, new List<Question>(), "test"));
            }
        }

        [Fact]
        public void ProvideFeedback_Fails_UnAuthorized()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Released;

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();

                // Create a new session to work with
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Mapper.CreateMap<Question, Question>();

                var questions = session.Questions.Select(Mapper.Map<Question>).ToList();

                questions[0].Feedbacks = new List<Feedback>
                    {
                        new Feedback
                            {
                                Created = DateTime.UtcNow,
                                Text = "This is new feedback",
                                Username = session.Reviewer
                            }
                    };

                Assert.Throws<AuthorizationException>(() => manager.ProvideFeedback(session.Id, questions, "FAIL"));
            }
        }

        [Fact]
        public void ProvideFeedback_Fails_Invalid_Status()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Archived;

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();

                // Create a new session to work with
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<InvalidOperationException>(() => manager.ProvideFeedback(session.Id, new List<Question>(), session.Reviewer));
            }
        }

    }
}
