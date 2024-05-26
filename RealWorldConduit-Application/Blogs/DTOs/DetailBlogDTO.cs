using RealWorldConduit_Application.Users.DTOs;

namespace RealWorldConduit_Application.Blogs.DTOs
{
    public class DetailBlogDTO
    {
        public MinimalBlogDTO Blog { get; init; }
        public MinimalUserDTO Author { get; init; }
    }
}
