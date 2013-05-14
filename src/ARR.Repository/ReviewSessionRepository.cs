using ARR.Data.Entities;
using ARR.Repository.Patch;
using Raven.Client;
using System.Linq;

namespace ARR.Repository
{
    public class ReviewSessionRepository : AbstractRepository<ReviewSession>
    {
        public ReviewSessionRepository(IDocumentSession session) : base(session) { }

        public override ReviewSession GetByName(string name)
        {
            return Find((r) => r.Title == name).FirstOrDefault();
        }

        protected override void InitializePatchFunctions()
        {
            PatchDictionary.Add(ReviewSession.SaveQuestionnairePatch, ReviewSessionPatchCollection.GetSaveQuestionnairePatch);
        }
    }
}
