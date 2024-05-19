﻿namespace RealWorldConduit_Infrastructure.Commons.Base
{
    public class PagingRequestDTO<T>
    {
        public int PageIndex { get; set; }
        public int PageLimit { get; set; }
        public int ItemLength { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; }
    }
}
