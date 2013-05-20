using System;
using System.Linq;
using System.Text;
using ARR.Data.Entities;
using ARR.ReviewSessionManagement;
using System.Collections.Generic;
using System.Web.Http;


namespace ARR.API.Controllers
{
    public class QuestionsController : ApiController
    {
        private readonly IReviewSessionManager _manager;

        public QuestionsController(IReviewSessionManager manager)
        {
            _manager = manager;
        }

        // GET: /Questions/

        // PUT api/questions/5/save-questionnaire
        public void Put(int id, string patch, List<Question> questions)
        {
            var username = GetAPIUser();

            switch (patch)
            {
                case "complete-session":
                    _manager.CompleteQuestionnaire(id, questions, username);
                    break;
                case "save-questionnaire":
                    _manager.SaveQuestionnaire(id, questions, username);
                    break;
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
