using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealWorldConduit_Infrastructure.Commons.Base;
using System.Net;

namespace RealWorldConduit_Infrastructure.Middlewares
{
    public class RestfulAPIExceptionMiddleware : IExceptionHandler
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<RestfulAPIExceptionMiddleware> _logger;
        private const string EXCEPTION_MESSAGE = "An unhandled exception has occurred while executing the request.";

        public RestfulAPIExceptionMiddleware(ILogger<RestfulAPIExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception is Exception ? exception.Message : EXCEPTION_MESSAGE);

            var problemDetails = CreateProblemDetails(httpContext, exception);

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        private ProblemDetails CreateProblemDetails(in HttpContext httpContext, in Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            switch (exception)
            {
                case RestfulAPIException restfulAPIException:
                    httpContext.Response.StatusCode = (int)restfulAPIException.Data[RestfulAPIException.STATUS_CODE];
                    break;
                case NotImplementedException notImplementedException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Type = exception.GetType().Name,
                Title = "An unexpected error occurred",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };
        }
    }
}
