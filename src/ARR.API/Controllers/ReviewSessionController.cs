using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using ARR.Data.Entities;
using ARR.ReviewSessionManagement;

using System.Collections.Generic;

namespace ARR.API.Controllers
{
    public class ReviewSessionController : BaseController
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
        public HttpResponseMessage Post(ReviewSession session)
        {
            HttpResponseMessage response;
            var username = GetAPIUser();
            try
            {
                _manager.Create(session, username);
                response = GetResponse(session.Id.ToString());
            }
            catch (Exception e)
            {
                response = GetResponse(e);
            }

            return response;
        }

        // PUT api/reviewsession/5/assignreviewer
        public HttpResponseMessage Put(int id, string patch, ReviewSession session)
        {
            HttpResponseMessage response;
            var username = GetAPIUser();

            try
            {
                switch (patch)
                {
                    case "archive":
                        _manager.Archive(id, username);
                        break;
                    case "release-session":
                        _manager.Release(id, username);
                        break;
                    default:
                        _manager.Edit(session, username);
                        break;
                }

                response = GetResponse();
            }
            catch (Exception e)
            {
                response = GetResponse(e);
            }

            return response;
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
