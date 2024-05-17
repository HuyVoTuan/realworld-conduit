using Quartz;

namespace RealWorldConduit_Infrastructure.Services.Jobs
{
    public interface IJobService
    {
        public Task ExecuteNow<T>(string jobId, IDictionary<string, object> data = default, CancellationToken cancellationToken = default) where T : IJob;
        public Task ExecuteAt<T>(DateTimeOffset startAt, string jobId, IDictionary<string, object> data = default, CancellationToken cancellationToken = default) where T : IJob;
        public Task ExecuteWithCronSchedule<T>(string cronExpression, string referenceId, IDictionary<string, object> data = default, CancellationToken cancellationToken = default) where T : IJob;
    }
}
