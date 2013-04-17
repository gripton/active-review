using ARR.Data.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ARR.Prototype.API.Controllers
{
    public class ProfileController : ApiController
    {
        // GET api/profile
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/profile/5
        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Post(Profile profile)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/profile/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/profile/5
        public void Delete(int id)
        {
        }
    }
}
