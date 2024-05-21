using MediatR;
using RealWorldConduit_Infrastructure.Commons.Base;

namespace RealWorldConduit_Infrastructure.Commons
{
    public interface IRequestWithBaseResponseHandler<TRequest, TResponse> : IRequestHandler<TRequest, BaseResponse<TResponse>> where TRequest : IRequestWithBaseResponse<TResponse>
    {
    }

    public interface IRequestWithBaseResponseHandler<TRequest> : IRequestHandler<TRequest, BaseResponse> where TRequest : IRequestWithBaseResponse
    {
    }
}
