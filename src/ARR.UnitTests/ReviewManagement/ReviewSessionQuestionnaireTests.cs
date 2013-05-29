using System;
using System.Collections.Generic;

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
    public class ReviewSessionQuestionnaireTests : BaseUnitTest
    {

        [Fact]
        public void SaveQuestionnaire_Fails_Not_Found()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            ReviewSession session = null;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<SessionNotFoundException>(() => manager.SaveQuestionnaire(1, new List<Question>(), "test"));
        }

        [Fact]
        public void SaveQuestionnaire_Fails_UnAuthorized()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Released;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);
                
            Assert.Throws<AuthorizationException>(() => manager.SaveQuestionnaire(session.Id, new List<Question>(), "FAIL"));
        }

        [Fact]
        public void SaveQuestionnaire_Fails_Invalid_Status()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Completed;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<InvalidOperationException>(() => manager.SaveQuestionnaire(session.Id, new List<Question>(), session.Reviewer));
        }

      

        [Fact]
        public void CompleteQuestionnaire_Fails_Not_Found()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            ReviewSession session = null;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<SessionNotFoundException>(() => manager.CompleteQuestionnaire(1, new List<Question>(), "test"));
        }

        [Fact]
        public void CompleteQuestionnaire_Fails_UnAuthorized()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Released;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<AuthorizationException>(() => manager.CompleteQuestionnaire(session.Id, new List<Question>(), "FAIL"));
        }

        [Fact]
        public void CompleteQuestionnaire_Fails_Invalid_Status()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Completed;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);
            Assert.Throws<InvalidOperationException>(() => manager.CompleteQuestionnaire(session.Id, new List<Question>(), session.Reviewer));
        }

        [Fact]
        public void ProvideFeedback_Fails_Not_Found()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            ReviewSession session = null;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);

            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<SessionNotFoundException>(() => manager.ProvideFeedback(1, new List<Question>(), "test"));
        }

        [Fact]
        public void ProvideFeedback_Fails_UnAuthorized()
        {
            var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Released;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<AuthorizationException>(() => manager.ProvideFeedback(session.Id, session.Questions, "FAIL"));
            
        }

        [Fact]
        public void ProvideFeedback_Fails_Invalid_Status()
        {
           var mockSession = new Mock<IDocumentSession>();
            var sessionRepo = new Mock<AbstractRepository<ReviewSession>>(mockSession.Object);
            var eventRepo = new Mock<AbstractRepository<Event>>(mockSession.Object);

            // Create a new session to work with
            var session = NewReviewSession();
            session.Reviewer = "test@test.com";
            session.SessionStatus = SessionStatusType.Archived;
            sessionRepo.Setup(r => r.Get(It.IsAny<int>())).Returns(session);


            var manager = new ReviewSessionManager(sessionRepo.Object, eventRepo.Object);

            Assert.Throws<InvalidOperationException>(() => manager.ProvideFeedback(session.Id, new List<Question>(), session.Reviewer));
        }

    }
}
