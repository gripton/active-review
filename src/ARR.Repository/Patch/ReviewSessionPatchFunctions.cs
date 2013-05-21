using System.Collections.Generic;
using ARR.Data.Entities;
using Raven.Abstractions.Data;
using Raven.Json.Linq;

namespace ARR.Repository.Patch
{
    public class ReviewSessionPatchCollection
    {
        public static PatchRequest[] GetSaveQuestionnairePatch(ReviewSession session)
        {
            return new[] { new PatchRequest {
                    Type = PatchCommandType.Set, 
                    Name = "Questions",
                    Value = RavenJObject.FromObject(session)
                }
            };
        }
    }
}
