using System;
using System.Linq;
using System.Text;
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
            var username = Request.Headers.Authorization.Parameter;
            _manager.Create(session, username);
        }

        // POST api/reviewsession/5
        public void Post(ReviewSession session, int id)
        {
            var currentUser = GetAPIUser();
            _manager.Edit(session, currentUser);
        }

        // PUT api/reviewsession/5/assignreviewer
        public void Put(int id, string patch, ReviewSession session)
        {
            var username = Request.Headers.Authorization.Parameter;

            switch (patch)
            {
                default:
                    _manager.Edit(session, username);
                    break;
            }
        }

        // DELETE api/reviewsession/5
        public void Delete(int id)
        {
            var username = Request.Headers.Authorization.Parameter;
            _manager.Delete(id, username);
        }

        // Temporary for handling security
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
