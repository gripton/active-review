using ARR.Data.Entities;
using System;

namespace ARR.ReviewSessionManagement
{
    public interface IReviewSessionMonitor : IObserver<Event>
    {
        void OnAssignedReviewer(int reviewId, string username);
        void OnSessionReleased(int reviewId, string username);
        void OnQuestionnaireCompleted(int reviewId, string username);
    }
}
