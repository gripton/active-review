using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using ARR.API.Models;
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

            var orderSessions = qry.ToList().OrderByDescending(sessionId => sessionId.Id).ThenBy(sessionDate => sessionDate.LastModified);

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
        public HttpResponseMessage Post(ReviewIndex index)
        {
            var username = GetAPIUser();
            var session = index.ToNewSession();
            _manager.Create(session, username);
            
            // Sending back the id in the reponse for the index page
            var response = new HttpResponseMessage {Content = new StringContent(session.Id.ToString())};
            response.StatusCode = HttpStatusCode.OK;
            return response;    
        }

        // PUT api/reviewindex/5
        public void Put(int id, string patch, ReviewIndex index)
        {
            var username = GetAPIUser();

            switch (patch)
            {
                case "assign-reviewer":
                    _manager.AssignReviewer(id, index.Reviewer, username);
                    break;
                default:
                    _manager.Edit(index.ToSession(_manager.ReadContext), username);
                    break;
            }
        }

        // DELETE api/reviewindex/5
        public void Delete(int id)
        {
            var username = GetAPIUser();
            _manager.Delete(id, username);
        }

        // Very temporary for handling security
        private string GetAPIUser()
        {
            IEnumerable<string> headerVals;

            Request.Headers.TryGetValues("Authorization", out headerVals);
            var sAuthHeader = headerVals.First();
            var authHeaderTokens = sAuthHeader.Split();

            var encodedDataAsBytes = Convert.FromBase64String(authHeaderTokens[0]);
            return Encoding.ASCII.GetString(encodedDataAsBytes);
        }
    }
}
