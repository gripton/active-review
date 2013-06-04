using ARR.Data.Entities;
using System;

namespace ARR.ReviewSessionManagement
{
    public interface IReviewSessionMonitor : IObserver<Event>
    {
        void OnAssignedReviewer(int reviewId, Event evt);
        void OnSessionReleased(int reviewId, Event evt);
        void OnQuestionnaireCompleted(int reviewId, Event evt);
    }
}
