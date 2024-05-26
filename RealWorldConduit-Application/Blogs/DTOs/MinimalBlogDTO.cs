namespace RealWorldConduit_Application.Blogs.DTOs
{
    public class MinimalBlogDTO
    {
        public string Slug { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Content { get; init; }
        public List<string> TagList { get; init; }
        public bool IsFavorited { get; init; }
        public int FavoritesCount { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}
