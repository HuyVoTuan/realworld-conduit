using System.Net;

namespace RealWorldConduit_Infrastructure.Commons.Base
{
    public class BaseResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        // Base constructor
        public BaseResponse()
        {
        }

        // Data return only constructor
        public BaseResponse(T data)
        {
            Data = data;
        }
    }

    public class BaseResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }

        // Base constructor
        public BaseResponse()
        {
        }

        // HttpStatusCode return only constructor
        public BaseResponse(HttpStatusCode code)
        {
            Code = code;
        }
    }
}
