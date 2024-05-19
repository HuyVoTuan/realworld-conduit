using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RealWorldConduit_Infrastructure.Commons.Base
{
    public class CustomActionResult<T> : IActionResult
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(Data)
            {
                StatusCode = (int)StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}

