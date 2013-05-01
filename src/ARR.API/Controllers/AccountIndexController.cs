using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ARR.API.Controllers
{
    public class AccountIndexController : ApiController
    {
        // GET api/accountindex
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/accountindex/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/accountindex
        public void Post([FromBody]string value)
        {
        }

        // PUT api/accountindex/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/accountindex/5
        public void Delete(int id)
        {
        }
    }
}
