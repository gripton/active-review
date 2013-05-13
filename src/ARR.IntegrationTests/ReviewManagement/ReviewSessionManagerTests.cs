using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using Autofac;
using Xunit;

namespace ARR.IntegrationTests.ReviewManagement
{
    public class ReviewSessionManagerTests : BaseIntegrationTest
    {
        [Fact]
        public void AssignReviewer_Succeeds()
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

                Assert.DoesNotThrow(() => manager.AssignReviewer(session.Id, "test@test.com", session.Creator));
                Assert.True(session.PendingReviewer);

                var events = eventRepo.List(e => e.EventType == EventType.ReviewerInvited);

                Assert.Equal(1, events.Count);
                Assert.Equal(events[0].EventType, EventType.ReviewerInvited);
                Assert.Equal(events[0].EntityId, session.Id);
            }
            
        }

        [Fact]
        public void AssignReviewer_Fails_Authorization()
        {

        }

        [Fact]
        public void AssignReviewer_Fails_NotFound()
        {

        }

        [Fact]
        public void AssignReviewer_Fails_To_Trigger_Event()
        {

        }

        [Fact]
        public void AssignReviewer_Fails_Pending_Session()
        {

        }

        [Fact]
        public void CreateNewSession_Succeeds()
        {

        }

        [Fact]
        public void CreateNewSession_Fails_Authorization()
        {

        }

        [Fact]
        public void CreateNewSession_Fails_InvalidChars()
        {

        }

        [Fact]
        public void CreateNewSession_Fails_With_ID()
        {

        }

        [Fact]
        public void DeleteSession_Succeeds()
        {

        }

        [Fact]
        public void DeleteSession_Fails_Authorization()
        {

        }

        [Fact]
        public void DeleteSession_Fails_In_Released_Status()
        {

        }

        [Fact]
        public void DeleteSession_Fails_NotFound()
        {

        }


    }
}