using System.Collections.Generic;
using ARR.Data.Entities;
using ARR.Repository;

namespace ARR.ReviewSessionManagement
{
    public interface IReviewSessionManager
    {
        void AssignReviewer(int sessionId, string reviewer, string current);
        void Create(ReviewSession session, string current);
        void Delete(int sessionId, string current);
        void Edit(ReviewSession session, string current);
        void Release(int sessionId, string current);
        void SaveQuestionnaire(int sessionId, List<Question> questions, string current);
        void CompleteQuestionnaire(int sessionId, List<Question> questions, string current);
        void ProvideFeedback(string content, int sessionId, string current);
        void Archive(int sessionId, string current);
        
        IReadContext<ReviewSession> ReadContext { get; }
    }
}
