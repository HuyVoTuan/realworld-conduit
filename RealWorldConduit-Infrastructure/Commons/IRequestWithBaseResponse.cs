using MediatR;
using RealWorldConduit_Infrastructure.Commons.Base;

namespace RealWorldConduit_Infrastructure.Commons
{
    public interface IRequestWithBaseResponse<T> : IRequest<BaseResponse<T>>
    {
    }

    public interface IRequestWithBaseResponse : IRequest<BaseResponse>
    {
    }
}
