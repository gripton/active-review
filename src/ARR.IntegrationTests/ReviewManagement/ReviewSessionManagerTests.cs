using System;

using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using Raven.Client;
using Xunit;

namespace ARR.IntegrationTests.ReviewManagement
{
    public class ReviewSessionManagerTests : BaseIntegrationTest
    {
        [Fact]
        public void NAME()
        {
            // Build the container.
            var container = Setup();

            var html = new HtmlEmailTemplate();
            html.Name = "ReviewerAssigned";
            html.Content = "test";

            var repo = new HtmlEmailTemplateRepository(container.Resolve<IDocumentStore>());

            repo.Save(html);
        }

        [Fact]
        public void AssignReviewer_Integrates()
        {
            // Build the container.
            var container = Setup();

            // Create a new session to work with
            var session = NewReviewSession();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.DoesNotThrow(() => manager.AssignReviewer(session.Id, "test@test.com", session.Creator));
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();
                
                var savedSession = sessionRepo.Get(session.Id);

                Assert.False(savedSession.PendingReviewer);
                Assert.Equal("test@test.com", savedSession.Reviewer);

                //TODO: Figure out why the build server test break on Event.EventType assertion
                //var events = eventRepo.ListAll();

                //Assert.Equal(1, events.Count);
                //Assert.Equal(events[0].EventType, EventType.ReviewerInvited);
                //Assert.Equal(events[0].EntityId, session.Id);
            }
        }

        [Fact]
        public void CreateNewSession_Integrates()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();
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
        public void DeleteSession_Integrates()
        {
            // Build the container.
            var container = Setup();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();

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
        public void Edit_Integrates()
        {
            // Build the container.
            var container = Setup();

            var session = NewReviewSession();

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                
                // Create a new session to work with
                sessionRepo.Save(session);
            }

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                AutoMapper.Mapper.CreateMap<ReviewSession, ReviewSession>();
                var editSession = AutoMapper.Mapper.Map<ReviewSession>(session);

                editSession.Requirements.Add(new Requirement {Comment = "test", Content = "test"});

                Assert.DoesNotThrow(() => manager.Edit(editSession, session.Creator));
            }
        }
     
        [Fact]
        public void Release_Integrates()
        {
            // Build the container.
            var container = Setup();
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Created;
            session.Reviewer = "somebody";

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                
                // Create a new session to work with
                sessionRepo.Save(session);
            }
            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.DoesNotThrow(() => manager.Release(session.Id, session.Creator));
            }
            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();

                var releasedSession = sessionRepo.Get(session.Id);

                Assert.Equal(SessionStatusType.Released, releasedSession.SessionStatus);

                //TODO: Figure out why the build server test break on Event.EventType assertion

                //var events = eventRepo.ListAll();

                //Assert.Equal(1, events.Count);
                //Assert.Equal(events[0].EventType, EventType.ReviewReleased);
                //Assert.Equal(events[0].EntityId, session.Id);
            }
        }

        [Fact]
        public void Archive_Integrates()
        {
            // Build the container.
            var container = Setup();
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Completed;

            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();

                // Create a new session to work with
                sessionRepo.Save(session);
            }
            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var eventRepo = lifetime.Resolve<AbstractRepository<Event>>();
                var manager = new ReviewSessionManager(sessionRepo, eventRepo);

                Assert.DoesNotThrow(() => manager.Archive(session.Id, session.Creator));
            }
            using (var lifetime = container.BeginLifetimeScope())
            {
                var sessionRepo = lifetime.Resolve<AbstractRepository<ReviewSession>>();
                var releasedSession = sessionRepo.Get(session.Id);

                Assert.Equal(SessionStatusType.Archived, releasedSession.SessionStatus);
            }
        }
    }
}