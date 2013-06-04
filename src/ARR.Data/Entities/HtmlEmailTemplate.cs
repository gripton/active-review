namespace ARR.Data.Entities
{
    public class HtmlEmailTemplate : IPersistentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}