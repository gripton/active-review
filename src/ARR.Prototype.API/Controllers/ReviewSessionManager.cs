using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ARR.Data.Entities;

namespace ARR.Prototype.API.Controllers
{
    public interface IReviewSessionManager
    {
        void Process(ReviewSession session);
        List<ReviewSession> ListCreated();
    }

    public class ReviewSessionManager : IReviewSessionManager
    {
        private readonly INotificationGenerator _generator;
        private readonly INotificationSender _sender;
        private readonly IReviewSessionMonitor _monitor;
        private readonly IReviewSessionRepository _repository;

        public ReviewSessionManager(INotificationGenerator generator, INotificationSender sender, 
            IReviewSessionRepository repository, IReviewSessionMonitor monitor)
        {
            _generator = generator;
            _sender = sender;
            _monitor = monitor;
            _repository = repository;
        }

        public List<ReviewSession> ListCreated()
        {
            return _repository.Get();
        }

        public void Process(ReviewSession session)
        {
            _repository.SaveReviewSession(session);

            MailMessage message = _generator.CreateContentAddedNotification();
            _sender.SendNotification(message);
        }
    }
}