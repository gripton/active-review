using ARR.Data.Entities;
using ARR.ReviewSessionManagement;
using System.Collections.Generic;
using System.Web.Http;

namespace ARR.API.Controllers
{
    public class ReviewSessionController : ApiController
    {
        private readonly IReviewSessionManager _manager;

        public ReviewSessionController(IReviewSessionManager manager)
        {
            _manager = manager;
        }

        // GET api/reviewsession
        public IEnumerable<ReviewSession> Get()
        {
            return _manager.ReadContext.ListAll();
        }

        // GET api/reviewsession/5
        public ReviewSession Get(int id)
        {
            return _manager.ReadContext.Get(id);
        }

        // POST api/reviewsession
        public void Post(ReviewSession session)
        {
            _manager.CreateNew(session);
        }

        // PUT api/reviewsession/5/patch
        public void Put(int id, string patch, ReviewSession session)
        {
            
        }

        // DELETE api/reviewsession/5
        public void Delete(int id)
        {
            _manager.Delete(id);
        }
    }
}
