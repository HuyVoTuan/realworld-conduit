namespace RealWorldConduit_Infrastructure.Services.Auth
{
    public interface ICurrentUserService
    {
        public Guid? Id { get; }
        public string? Slug { get; }
    }
}
