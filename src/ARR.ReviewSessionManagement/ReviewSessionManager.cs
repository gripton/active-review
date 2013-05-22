using System;
using System.Collections.Generic;

using ARR.Core.Authorization;
using ARR.Data.Entities;
using ARR.Repository;
using ARR.ReviewSessionManagement.Exceptions;

namespace ARR.ReviewSessionManagement
{
    public class ReviewSessionManager : IReviewSessionManager
    {
        private readonly ReviewSessionRepository _sessionRepository;
        private readonly EventRepository _eventRepository;


        public ReviewSessionManager(ReviewSessionRepository sessionRepository, EventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            _sessionRepository = sessionRepository;

            ReadContext = sessionRepository;
        }

        public IReadContext<ReviewSession> ReadContext { get; private set; }

        /// <summary>
        /// If the ‘username’ exists, assigns the reviewer’s username to the session in the system. If not, triggers an event 
        /// which will send out an email to invite a new reviewer to register an account with the system.
        /// </summary>
        /// <remarks>
        /// The event persist the session id for which the new user will be assign as a reviewer. 
        /// This will be used by the Account Monitor to assign the new reviewer’s username to the session in the system.
        /// </remarks>
        /// <param name="reviewer">The username of the reviewer</param>
        /// <param name="sessionId">The id of the review session</param>
        /// <param name="current">The API user's username</param>
        /// <exception cref="SessionNotFoundException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void AssignReviewer(int sessionId, string reviewer, string current)
        {
            var session = _sessionRepository.Get(sessionId);

            if (session == null)
                throw new SessionNotFoundException();

            if (session.PendingReviewer)
                throw new InvalidOperationException();

            if (session.Creator.ToLower() != current.ToLower())
                throw new AuthorizationException();

            session.PendingReviewer = true;
            _sessionRepository.Save(session);

            var assignEvent = new Event
            {
                Created = DateTime.UtcNow,
                EntityId = session.Id,
                EventType = EventType.ReviewerInvited,
                Info = new Dictionary<string, string> { { "username", reviewer } }
            };

            _eventRepository.Save(assignEvent);
        }

        /// <summary>
        /// Creates a new review session within the system.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="current"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Create(ReviewSession session, string current)
        {
            if (session.Id != 0)
                throw new InvalidOperationException("Cannot create a new session with the 'Id' field already populated.");

            session.Creator = current;
            session.LastModified = DateTime.UtcNow;
            session.SessionStatus = SessionStatusType.Created;
            
            _sessionRepository.Save(session);

        }

        /// <summary>
        /// Remove a session from the system.
        /// </summary>
        /// <param name="sessionId">The id of the review session</param>
        /// <param name="current">The API user's username</param>
        /// <exception cref="SessionNotFoundException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Delete(int sessionId, string current)
        {
            var session = _sessionRepository.Get(sessionId);

            if (session == null) return;

            if(session.SessionStatus == SessionStatusType.Released)
                throw new InvalidOperationException();

            if (session.Creator.ToLower() != current.ToLower())
                throw new AuthorizationException();

            _sessionRepository.Delete(session);
        }

        /// <summary>
        /// Updates an existing session.
        /// </summary>
        /// <remarks>Saving after session has been released will be disallowed as only feedback and answers will be provided
        /// until the session is archived. There are seperate manager operations to handle these operations.</remarks>
        /// <param name="session"></param>
        /// <param name="current">The username of the API user</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        public void Edit(ReviewSession session, string current)
        {
            if (session.Id == 0)
                throw new InvalidOperationException("Cannot create a new session with the 'Id' field already populated.");

            if (session.SessionStatus != SessionStatusType.Created)
                throw new InvalidOperationException();

            if (session.Creator.ToLower() != current.ToLower())
                throw new AuthorizationException();

            session.LastModified = DateTime.UtcNow;
            _sessionRepository.Save(session);
        }

        /// <summary>
        /// Updates the status of a review to “Released” status.
        /// </summary>
        /// <remarks>Triggers an event to notify the reviewer that the questions are ready to be answered.</remarks>
        /// <param name="sessionId"></param>
        /// <param name="current">The username of the API user</param>
        /// <exception cref="SessionNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        public void Release(int sessionId, string current)
        {
            var session = _sessionRepository.Get(sessionId);

            if (session == null)
                throw new SessionNotFoundException();
           
            if (session.SessionStatus != SessionStatusType.Created)
                throw new InvalidOperationException();
            
            if (session.Creator.ToLower() != current.ToLower())
                throw new AuthorizationException();

            session.SessionStatus = SessionStatusType.Released;
            session.LastModified = DateTime.UtcNow;
            _sessionRepository.Save(session);

            var assignEvent = new Event
            {
                Created = DateTime.UtcNow,
                EntityId = session.Id,
                EventType = EventType.ReviewReleased
            };

            _eventRepository.Save(assignEvent);
        }
       
        /// <summary>
        /// Accepts answers for a review session question. Updates the review session.
        /// </summary>
        /// <param name="sessionId">The id of the review session</param>
        /// <param name="questions">The questions of the review session</param>
        /// <param name="current">The username of the API user</param>
        /// <exception cref="InvalidWebCharacterException"></exception>
        /// <exception cref="SessionNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        public void SaveQuestionnaire(int sessionId, List<Question> questions, string current)
        {
            SaveQuestionnaire(sessionId, questions, current, SessionStatusType.Released);
        }

        /// <summary>
        /// Performs the save questionnaire logic, updates the status of a review to “completed” status, 
        /// and triggers an event to notify the reviewer that the questions are ready to be answered. 
        /// </summary>
        /// <param name="sessionId">The id of the review session</param>
        /// <param name="questions"></param>
        /// <param name="current">The username of the API user</param>
        /// <exception cref="InvalidWebCharacterException"></exception>
        /// <exception cref="SessionNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        public void CompleteQuestionnaire(int sessionId, List<Question> questions, string current)
        {
            SaveQuestionnaire(sessionId, questions, current, SessionStatusType.Completed);

            var assignEvent = new Event
            {
                Created = DateTime.UtcNow,
                EntityId = sessionId,
                EventType = EventType.QuestionnaireCompleted
            };

            _eventRepository.Save(assignEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">The feedback text</param>
        /// <param name="sessionId">The id of the review session</param>
        /// <param name="current">The username of the API user</param>
        /// <exception cref="InvalidWebCharacterException"></exception>
        /// <exception cref="SessionNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        public void ProvideFeedback(string content, int sessionId, string current)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="current">The username of the API user</param>
        /// <exception cref="SessionNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        public void Archive(int sessionId, string current)
        {
            var session = _sessionRepository.Get(sessionId);

            if (session == null)
                throw new SessionNotFoundException();

            if (session.SessionStatus != SessionStatusType.Completed)
                throw new InvalidOperationException();

            if (session.Creator.ToLower() != current.ToLower())
                throw new AuthorizationException();

            session.SessionStatus = SessionStatusType.Archived;
            session.LastModified = DateTime.UtcNow;
            _sessionRepository.Save(session);
        }


        private void SaveQuestionnaire(int sessionId, List<Question> questions, string current, SessionStatusType sessionStatus)
        {
            var session = _sessionRepository.Get(sessionId);
            session.Questions = questions;

            if (session == null)
                throw new SessionNotFoundException();
            
            if (session.Reviewer.ToLower() != current.ToLower())
                throw new AuthorizationException();

            if (session.SessionStatus != SessionStatusType.Released)
                throw new InvalidOperationException();

            // TODO - these won't work unless we add patches for them
            session.SessionStatus = sessionStatus;
            session.LastModified = DateTime.UtcNow;

            session.Title = "dont change me";

            _sessionRepository.Patch(session, ReviewSession.SaveQuestionnairePatch);
        }

        private bool HasValidWebCharacters(ReviewSession session)
        {
            return true;
        }
    }
}
