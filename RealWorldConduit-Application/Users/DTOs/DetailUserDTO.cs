namespace RealWorldConduit_Application.Users.DTOs
{
    public class DetailUserDTO
    {
        public string Slug { get; init; }
        public string AvatarUrl { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string Bio { get; init; }
        public ICollection<MinimalUserDTO> Friends { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}
