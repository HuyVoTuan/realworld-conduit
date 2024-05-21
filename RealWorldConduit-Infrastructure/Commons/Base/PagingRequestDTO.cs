namespace RealWorldConduit_Infrastructure.Commons.Base
{
    public class PagingRequestDTO
    {
        public string PageIndex { get; init; } = "1";
        public string PageLimit { get; init; } = "5";
    }
}
