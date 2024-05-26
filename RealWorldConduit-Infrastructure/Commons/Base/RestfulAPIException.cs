using System.Collections;
using System.Net;

namespace RealWorldConduit_Infrastructure.Commons.Base
{

    public class RestfulAPIException : Exception
    {
        public static string STATUS_CODE = "status_code";
        public HttpStatusCode _httpStatusCode { get; }
        public IDictionary<string, IEnumerable<string>> _errors { get; }

        public RestfulAPIException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            _httpStatusCode = httpStatusCode;
        }

        public override IDictionary Data => new Dictionary<string, int>()
        {
            {STATUS_CODE, (int)_httpStatusCode }
        };
    }
}

