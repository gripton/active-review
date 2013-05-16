using System.Web.Http;
using ARR.Data.Entities;
using ARR.ReviewSessionManagement;

namespace ARR.API.Controllers
{
    public class FeedbackController : ApiController
    {
        private readonly IReviewSessionManager _manager;

        public FeedbackController(IReviewSessionManager manager)
        {
            _manager = manager;
        }

        // POST api/forum
        public void Post(int id, Question question)
        {
            //TODO: Just making sure I can get here...
            //_manager.ProvideFeedback();
        }

        // DELETE api/forum/5
        public void Delete(int id)
        {
        }
    }
}
