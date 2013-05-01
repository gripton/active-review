using ARR.Data.Entities;
using ARR.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.ReviewSessionManagement
{
    public interface IReviewSessionManager
    {
        void CreateNew(ReviewSession session);
        void Save(ReviewSession session);
        void Delete(int id);

        IReadContext<ReviewSession> ReadContext { get; }
    }
}
