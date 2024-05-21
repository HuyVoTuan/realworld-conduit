namespace RealWorldConduit_Infrastructure.Commons.Base
{
    public class PagingResponseDTO<T>
    {
        public int PageIndex { get; init; }
        public int PageLimit { get; init; }
        public int ItemLength { get; init; }
        public int TotalPages { get; init; }
        public List<T> Data { get; init; }
    }
}
