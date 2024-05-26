using RealWorldConduit_Application.Users.DTOs;

namespace RealWorldConduit_Application.Blogs.DTOs
{
    public class MinimalCommentDTO
    {
        public string Content { get; init; }
        public MinimalUserDTO Author { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}
