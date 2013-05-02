using ARR.Prototype.API.Models;
using ARR.ReviewSessionManagement;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ARR.API.Controllers
{
    public class ReviewIndexController : ApiController
    {
        private readonly IReviewSessionManager _manager;

        public ReviewIndexController(IReviewSessionManager manager)
        {
            _manager = manager;
        }

        // GET api/reviewindex
        public IEnumerable<ReviewIndex> Get()
        {
            var indexes = new List<ReviewIndex>();
            var sessions = _manager.ReadContext.ListAll();

            foreach (var session in sessions)
            {
                indexes.Add(Mapper.Map<ReviewIndex>(session));
            }

            return indexes;
        }

        // GET api/reviewindex/5
        public ReviewIndex Get(int id)
        {
            var session = _manager.ReadContext.Get(id);
            var index = Mapper.Map<ReviewIndex>(session);
            return index;
        }

        // POST api/reviewindex
        public void Post(ReviewIndex session)
        {
        }

        // PUT api/reviewindex/5
        public void Put(int id, string patch, ReviewIndex account)
        {
        }

        // DELETE api/reviewindex/5
        public void Delete(int id)
        {
        }
    }
}
