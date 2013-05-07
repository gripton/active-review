using System.Linq;
using ARR.API.Models;
using ARR.Data.Entities;
using ARR.ReviewSessionManagement;
using AutoMapper;
using System.Collections.Generic;
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
            var qry = from reviewSession in _manager.ReadContext.FindAll() 
                        orderby reviewSession.LastModified ascending
                        select reviewSession;

            var orderSessions = qry.ToList();

            return orderSessions.Select(Mapper.Map<ReviewIndex>).ToList();
        }

        // GET api/reviewindex/5
        public ReviewIndex Get(int id)
        {
            var session = _manager.ReadContext.Get(id);
            var index = Mapper.Map<ReviewIndex>(session);
            return index;
        }

        // POST api/reviewindex
        public void Post(ReviewIndex index)
        {
            var session = new ReviewSession();
            _manager.Save(index.ToNewSession());
        }

        // PUT api/reviewindex/5
        public void Put(int id, string patch, ReviewIndex index)
        {
            switch (patch)
            {
                case "assign-reviewer":
                    _manager.AssignReviewer(index.Reviewer, id);
                    break;
                default:
                    _manager.Save(index.ToSession(_manager.ReadContext));
                    break;
            }
        }

        // DELETE api/reviewindex/5
        public void Delete(int id)
        {
            _manager.Delete(id);
        }        
    }
}
