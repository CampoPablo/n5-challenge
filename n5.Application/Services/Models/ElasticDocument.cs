namespace n5.Application.Services.Models
{
    public class ElasticDocument
    {
        public ElasticDocument(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
