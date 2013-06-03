using ARR.Data.Entities;
using System;
using ARR.Notifications;
using ARR.Repository;

namespace ARR.ReviewSessionManagement
{
    public class ReviewSessionMonitor : IReviewSessionMonitor
    {
        public ReviewSessionMonitor(AbstractRepository<ReviewSession> reviewRepository,
            AbstractRepository<Event> eventRepository,
            INotificationGenerator generator, INotificationSender sender)
        {

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
            //throw new NotImplementedException();
        }

        public void OnAssignedReviewer(int reviewId, string username)
        {
            throw new NotImplementedException();
        }

        public void OnSessionReleased(int reviewId, string username)
        {
            throw new NotImplementedException();
        }

        public void OnQuestionnaireCompleted(int reviewId, string username)
        {
            throw new NotImplementedException();
        }
    }
}
