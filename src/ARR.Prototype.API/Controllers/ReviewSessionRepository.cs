using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARR.Data.Entities;
using Raven.Client;

namespace ARR.Prototype.API.Controllers
{
    public interface IReviewSessionRepository
    {
        void SaveReviewSession(ReviewSession session);
    }
    
    public class ReviewSessionRepository : IReviewSessionRepository
    {
        private readonly IDocumentSession _session;
        public ReviewSessionRepository(IDocumentSession session)
        {
            _session = session;
        }

        public void SaveReviewSession(ReviewSession reviewSession)
        {
            // Flush those changes
            _session.Store(reviewSession);
            _session.SaveChanges();
        }
    }
}