using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using ARR.Data.Entities;
using ARR.Notifications;
using ARR.Repository;

namespace ARR.ReviewSessionManagement
{
    public class ReviewSessionManager : IReviewSessionManager
    {
        private readonly INotificationGenerator _generator;
        private readonly INotificationSender _sender;
        private readonly IReviewSessionMonitor _monitor;
        private readonly ReviewSessionRepository _repository;

        public ReviewSessionManager(INotificationGenerator generator, INotificationSender sender,
            ReviewSessionRepository repository, IReviewSessionMonitor monitor)
        {
            _generator = generator;
            _sender = sender;
            _monitor = monitor;
            _repository = repository;
        }

        public IReadContext<ReviewSession> ReadContext { get; private set; }

        public void CreateNew(ReviewSession session)
        {
            _repository.Save(session);
            // MailMessage message = _generator.();
            //_sender.SendNotification(message);
        }

        public void Save(ReviewSession session)
        {
            _repository.Save(session);
        }

        public void Delete(int id)
        {
            var account = _repository.Get(id);
            _repository.Delete(account);
        }        
    }
}
