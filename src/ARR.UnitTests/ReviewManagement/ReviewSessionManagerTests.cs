using System;

using ARR.Core.Authorization;
using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement;
using ARR.ReviewSessionManagement.Exceptions;

using Moq;
using Raven.Client;
using Xunit;

namespace ARR.UnitTests.ReviewManagement
{
    public class ReviewSessionManagerTests : BaseUnitTest
    {

        [Fact]
        public void AssignReviewer_Fails_Authorization()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<AuthorizationException>(() => manager.AssignReviewer(session.Id, "test@test.com", "FAIL"));
        }

        [Fact]
        public void AssignReviewer_Fails_NotFound()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            ReviewSession session = null;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<SessionNotFoundException>(() => manager.AssignReviewer(-1, "test@test.com", "test@test.com"));
        }

        [Fact]
        public void AssignReviewer_Fails_Pending_Session()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.PendingReviewer = true;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);
            Assert.Throws<InvalidOperationException>(() => manager.AssignReviewer(session.Id, "test@test.com", session.Creator));
        }

        [Fact]
        public void CreateNewSession_Fails_With_ID()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.Id = 1;
            
            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);
            Assert.Throws<InvalidOperationException>(() => manager.Create(session, "test_user"));
        }

        [Fact]
        public void DeleteSession_Fails_Authorization()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<AuthorizationException>(() => manager.Delete(session.Id, "test"));
        }

        [Fact]
        public void DeleteSession_Fails_In_Invalid_Status()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Released;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<InvalidOperationException>(() => manager.Delete(session.Id, "test"));
        }

        [Fact]
        public void Edit_Fails_UnAuthorized()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<AuthorizationException>(() => manager.Edit(session, "FAIL"));
        }

        [Fact]
        public void Edit_Fails_Invalid_Status()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Released;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<InvalidOperationException>(() => manager.Edit(session, session.Creator));
        }

        [Fact]
        public void Release_Fails_Not_Found()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            ReviewSession session = null;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<SessionNotFoundException>(() => manager.Release(1, "test"));
        }

        [Fact]
        public void Release_Fails_UnAuthorized()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Created;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<AuthorizationException>(() => manager.Release(session.Id, "FAIL"));
        }

        [Fact]
        public void Release_Fails_Invalid_Status()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Completed;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<InvalidOperationException>(() => manager.Release(session.Id, session.Creator));
        }

        [Fact]
        public void Archive_Fails_Not_Found()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            ReviewSession session = null;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<SessionNotFoundException>(() => manager.Archive(1, "test"));

        }

        [Fact]
        public void Archive_Fails_UnAuthorized()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Completed;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<AuthorizationException>(() => manager.Archive(session.Id, "FAIL"));
        }

        [Fact]
        public void Archive_Fails_Invalid_Status()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.SessionStatus = SessionStatusType.Released;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<InvalidOperationException>(() => manager.Archive(session.Id, session.Creator));
        }
    }
}