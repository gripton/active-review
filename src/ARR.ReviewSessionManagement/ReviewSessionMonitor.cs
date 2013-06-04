using System;
using System.Configuration;
using System.Net.Mail;
using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;
using NLog;

namespace ARR.ReviewSessionManagement
{
    public interface IReviewSessionMonitor : IObserver<Event> { }

    public class ReviewSessionMonitor : IReviewSessionMonitor
    {
        private static readonly Logger log = LogManager.GetLogger(typeof(ReviewSessionMonitor).Name);

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
                case EventType.ReviewSessionReleased:
                case EventType.QuestionnaireCompleted:
                    try
                    {
                        var message = GenerateMessage(value.EntityId, value);
                        _sender.SendNotification(message);
                        value.Recevied = true;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Email Notification Failed", ex);
                    }
                    break;
                default:
                    return;
            }
        }

        private MailMessage GenerateMessage(int reviewId, Event evt)
        {
            MailMessage message;
            try
            {
                var session = _reviewRepository.Get(reviewId);
                var account = _accountRepository.GetByName(session.Reviewer);

                var args = new NotificationArgs
                    {
                        BaseUrl = ConfigurationManager.AppSettings["WebUrl"],
                        DisplayName = account.ScreenName,
                        Recipient = account.EmailAddress,
                        SessionId = reviewId.ToString(),
                        SessionName = session.Title
                    };

                message = _generator.Generate(evt.EventType, args);
                evt.Recevied = true;
            }
            catch (Exception ex)
            {
                string test = ex.Message;
                throw;
            }

            return message;
        }
    }
}
