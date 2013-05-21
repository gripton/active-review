using System;
using ARR.Core.Authorization;
using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using ARR.ReviewSessionManagement.Exceptions;
using Autofac;
using Xunit;

namespace ARR.IntegrationTests.ReviewManagement
{
    public class ReviewSessionManagerTests : BaseIntegrationTest
    {
        [Fact]
        public void AssignReviewer_Succeeds()
        {
            // Test
            /* Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();

                // Create a new session to work with
                var session = NewReviewSession();
                sessionRepo.Save(session);

                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.DoesNotThrow(() => manager.AssignReviewer(session.Id, "test@test.com", session.Creator));
                Assert.True(session.PendingReviewer);

                var events = eventRepo.List(e => e.EventType == EventType.ReviewerInvited);

                Assert.Equal(1, events.Count);
                Assert.Equal(events[0].EventType, EventType.ReviewerInvited);
                Assert.Equal(events[0].EntityId, session.Id);
            }*/
        }

        [Fact]
        public void AssignReviewer_Fails_Authorization()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();

                // Create a new session to work with
                var session = NewReviewSession();
                sessionRepo.Save(session);

                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<AuthorizationException>(() => manager.AssignReviewer(session.Id, "test@test.com", "FAIL"));
                
                // Make sure it doesn't create any events
                var events = eventRepo.List(e => e.EventType == EventType.ReviewerInvited);
                Assert.Equal(0, events.Count);
            }
        }

        [Fact]
        public void AssignReviewer_Fails_NotFound()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();

                // Create a new session to work with
                var session = NewReviewSession();
                sessionRepo.Save(session);

                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<SessionNotFoundException>(() => manager.AssignReviewer(-1, "test@test.com",session.Creator));

                // Make sure it doesn't create any events
                var events = eventRepo.List(e => e.EventType == EventType.ReviewerInvited);
                Assert.Equal(0, events.Count);
            }
        }

        [Fact]
        public void AssignReviewer_Fails_Pending_Session()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();

                // Create a new session to work with
                var session = NewReviewSession();
                session.PendingReviewer = true;
                sessionRepo.Save(session);

                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<InvalidOperationException>(() => manager.AssignReviewer(session.Id, "test@test.com", session.Creator));

                // Make sure it doesn't create any events
                var events = eventRepo.List(e => e.EventType == EventType.ReviewerInvited);
                Assert.Equal(0, events.Count);
            }
        }

        [Fact]
        public void CreateNewSession_Succeeds()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                var session = NewReviewSession();

                Assert.DoesNotThrow(() => manager.Create(session, "test_user"));
                Assert.True(session.Id > 0);
                Assert.Equal(session.Creator, "test_user");
                Assert.Equal(SessionStatusType.Created, session.SessionStatus);
                Assert.InRange(session.LastModified.Ticks, DateTime.UtcNow.AddMinutes(-1).Ticks, DateTime.UtcNow.AddMinutes(1).Ticks);
            }
        }
        

        [Fact]
        public void CreateNewSession_Fails_InvalidChars()
        {

        }

        [Fact]
        public void CreateNewSession_Fails_With_ID()
        {
             var container = Setup();

             using (var lifetime = container.BeginLifetimeScope())
             {
                 var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                 var eventRepo = lifetime.Resolve<EventRepository>();
                 var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                 var session = NewReviewSession();
                 session.Id = 1;

                 Assert.Throws<InvalidOperationException>(() => manager.Create(session, "test_user"));
             }
        }

        [Fact]
        public void Edit_Succeeds()
        {
            /* Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                // Create a new session to work with
                var session = NewReviewSession();
                sessionRepo.Save(session);

                AutoMapper.Mapper.CreateMap<ReviewSession, ReviewSession>();
                var editSession = AutoMapper.Mapper.Map<ReviewSession>(session);

                editSession.Requirements.Add(new Requirement{Comment = "test", Content = "test"});

                Assert.DoesNotThrow(() => manager.Edit(editSession, session.Creator));
            }*/
        }

        [Fact]
        public void DeleteSession_Succeeds()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();

                // Create a new session to work with
                var session = NewReviewSession();
                sessionRepo.Save(session);

                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.DoesNotThrow(() => manager.Delete(session.Id, session.Creator));

                var nullSession = sessionRepo.Get(session.Id);
                Assert.Null(nullSession);

            }
        }

        [Fact]
        public void DeleteSession_Fails_Authorization()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();

                // Create a new session to work with
                var session = NewReviewSession();
                sessionRepo.Save(session);

                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<AuthorizationException>(() => manager.Delete(session.Id, "test"));

                var stillSession = sessionRepo.Get(session.Id);
                Assert.NotNull(stillSession);

            }
        }

        [Fact]
        public void DeleteSession_Fails_In_Released_Status()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<ReviewSessionRepository>();
                var eventRepo = lifetime.Resolve<EventRepository>();

                // Create a new session to work with
                var session = NewReviewSession();
                session.SessionStatus = SessionStatusType.Released;
                sessionRepo.Save(session);

                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.Throws<InvalidOperationException>(() => manager.Delete(session.Id, "test"));

                var stillSession = sessionRepo.Get(session.Id);
                Assert.NotNull(stillSession);

            }
        }

        [Fact]
        public void DeleteSession_Fails_NotFound()
        {

        }


    }
}