using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ARR.API.Controllers
{
    public class ReviewSessionController : ApiController
    {
        // GET api/reviewsession
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/reviewsession/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/reviewsession
        public HttpResponseMessage Post(dynamic data)
        {
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
