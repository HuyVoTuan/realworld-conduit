namespace RealWorldConduit_Application.Users.DTOs
{
    public class UserDTO
    {
        public string Slug { get; init; }
        public string AvatarUrl { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string Bio { get; init; }
        public LocationDTO Locations { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}
