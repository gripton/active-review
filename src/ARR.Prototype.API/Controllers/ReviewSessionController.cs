using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ARR.Data.Entities;
using ARR.Prototype.API.Controllers;

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
            return _manager.ListCreated();
        }

        // GET api/reviewsession/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/reviewsession
        public HttpResponseMessage Post(ReviewSession data)
        {
            _manager.Process(data);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/reviewsession/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/reviewsession/5
        public void Delete(int id)
        {
        }
    }
}
