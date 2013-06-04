using System;

using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;

namespace ARR.ReviewSessionManagement
{
    public class ReviewSessionMonitor : IReviewSessionMonitor
    {
        private readonly AbstractRepository<ReviewSession> _reviewRepository;
        private readonly AbstractRepository<Event> _eventRepository;
        private readonly INotificationGenerator _generator;
        private readonly INotificationSender _sender;
        private readonly AbstractRepository<Account> _accountRepository;

        public ReviewSessionMonitor(AbstractRepository<ReviewSession> reviewRepository,
            AbstractRepository<Account> accountRepository,
            AbstractRepository<Event> eventRepository,
            INotificationGenerator generator, INotificationSender sender)
        {
            _accountRepository = accountRepository;
            _sender = sender;
            _generator = generator;
            _eventRepository = eventRepository;
            _reviewRepository = reviewRepository;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Event value)
        {
            switch (value.EventType)
            {
                case EventType.ReviewerAssigned:
                    OnAssignedReviewer(value.EntityId, value);
                    break;
                case EventType.ReviewSessionReleased:
                    OnSessionReleased(value.EntityId, value);
                    
                    break;
                case EventType.QuestionnaireCompleted:
                    OnQuestionnaireCompleted(value.EntityId, value);
                    value.Recevied = true;
                    break;
                default:
                    return;
            }
        }

        public void OnAssignedReviewer(int reviewId, Event evt)
        {
            try
            {
                var session = _reviewRepository.Get(reviewId);
                var account = _accountRepository.GetByName(session.Reviewer);
                _sender.SendNotification(_generator.GenerateAssignedMessage(account.EmailAddress, session.Title));
                evt.Recevied = true;
            }
            catch(Exception ex)
            {
                string test = ex.Message;
                throw;
            }
            
        }

        public void OnSessionReleased(int reviewId, Event evt)
        {
            try
            {
                var session = _reviewRepository.Get(reviewId);
                var account = _accountRepository.GetByName(session.Reviewer);
                _sender.SendNotification(_generator.GenerateReleasedMessage(account.EmailAddress, reviewId, session.Title));
                evt.Recevied = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void OnQuestionnaireCompleted(int reviewId, Event evt)
        {
            try
            {
                var session = _reviewRepository.Get(reviewId);
                var account = _accountRepository.GetByName(session.Reviewer);
                _sender.SendNotification(_generator.GenerateCompletedMessage(account.EmailAddress, session.Title));
                evt.Recevied = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
