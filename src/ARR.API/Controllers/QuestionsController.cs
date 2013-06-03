using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using ARR.Data.Entities;
using ARR.ReviewSessionManagement;
using System.Collections.Generic;
using AutoMapper;


namespace ARR.API.Controllers
{
    public class QuestionsController : BaseController
    {
        private readonly IReviewSessionManager _manager;

        public QuestionsController(IReviewSessionManager manager)
        {
            _manager = manager;
        }

        // GET: /Questions/

        // PUT api/questions/5/save-questionnaire
        public HttpResponseMessage Put(int id, string patch, List<Question> questions)
        {
            var username = GetAPIUser();

            try
            {
                switch (patch)
                {
                    case "complete-session":
                        _manager.CompleteQuestionnaire(id, questions, username);
                        break;
                    case "save-questionnaire":
                        _manager.SaveQuestionnaire(id, questions, username);
                        break;
                    case "provide-feedback":
                        _manager.ProvideFeedback(id, questions, username);
                        break;
                }
                return GetResponse(id.ToNullSafeString());
            }
            catch (Exception e)
            {
                return GetResponse(e);
            }
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
