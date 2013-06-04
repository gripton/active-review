using System.Linq;
using ARR.Data.Entities;
using Raven.Client;

namespace ARR.Repository
{
    public class HtmlEmailTemplateRepository : AbstractRepository<HtmlEmailTemplate>
    {
        public HtmlEmailTemplateRepository(IDocumentSession session) : base(session) { }

        public HtmlEmailTemplateRepository(IDocumentStore store) : base(store) { }

        public override HtmlEmailTemplate GetByName(string name)
        {
            return Find((html) => html.Name == name).FirstOrDefault();
        }
    }
}