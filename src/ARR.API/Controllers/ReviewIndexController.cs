using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using ARR.API.Models;
using ARR.ReviewSessionManagement;
using AutoMapper;
using System.Collections.Generic;
using NLog;

namespace ARR.API.Controllers
{
    public class ReviewIndexController : BaseController
    {
        private readonly IReviewSessionManager _manager;
        private static readonly Logger log = LogManager.GetLogger(typeof(ReviewIndexController).Name);

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
            log.Debug("A new review session has been posted");

            HttpResponseMessage response;
            var username = GetAPIUser();
            var session = index.ToNewSession();
            
            try
            {
                _manager.Create(session, username);
                response = GetResponse(session.Id.ToNullSafeString());
            }
            catch (Exception e)
            {
                response = GetResponse(e);
            }

            return response;
        }

        // PUT api/reviewindex/5
        public HttpResponseMessage Put(int id, string patch, ReviewIndex index)
        {
            HttpResponseMessage response;
            var username = GetAPIUser();

            try
            {
                switch (patch)
                {
                    case "assign-reviewer":
                        _manager.AssignReviewer(id, index.Reviewer, username);
                        break;
                    default:
                        _manager.Edit(index.ToSession(_manager.ReadContext), username);
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

        // DELETE api/reviewindex/5
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;
            var username = GetAPIUser();

            try
            {
                _manager.Delete(id, username);
                response = GetResponse();
            }
            catch (Exception e)
            {
                response = GetResponse(e);
            }

            return response;
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
