using Microsoft.Extensions.Logging;
using Quartz;
using RealWorldConduit_Infrastructure.Helpers;

namespace RealWorldConduit_Infrastructure.Services.Jobs
{
    public class JobService : IJobService
    {
        private readonly ILogger<JobService> _logger;
        private readonly ISchedulerFactory _schedulerFactory;

        public JobService(ILogger<JobService> logger, ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
        }
        public async Task ExecuteAt<T>(DateTimeOffset startAt, string jobId, IDictionary<string, object> data = null, CancellationToken cancellationToken = default) where T : IJob
        {
            try
            {
                _logger.LogInformation($"Start to execute job {jobId}-{typeof(T).Name} now");
                IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                IJobDetail jobDetail;

                if (data is null)
                {
                    jobDetail = JobBuilder.Create<T>()
                                          .WithIdentity(StringHelper.GenerateJobId(jobId) + typeof(T).Name, typeof(T).Name)
                                          .Build();
                }
                else
                {
                    jobDetail = JobBuilder.Create<T>()
                                          .WithIdentity(StringHelper.GenerateJobId(jobId) + typeof(T).Name, typeof(T).Name)
                                          .UsingJobData(new JobDataMap(data))
                                          .Build();
                }

                ITrigger trigger = TriggerBuilder.Create()
                                                 .WithIdentity(StringHelper.GenerateJobTriggerId(jobId) + typeof(T).Name, typeof(T).Name)
                                                 .StartAt(startAt)
                                                 .Build();

                await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
                await scheduler.Start(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task ExecuteNow<T>(string jobId, IDictionary<string, object> data = null, CancellationToken cancellationToken = default) where T : IJob
        {
            try
            {
                _logger.LogInformation($"Start to execute job {jobId}-{typeof(T).Name} now");
                IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                IJobDetail jobDetail;

                if (data is null)
                {
                    jobDetail = JobBuilder.Create<T>()
                                          .WithIdentity(StringHelper.GenerateJobId(jobId) + typeof(T).Name, typeof(T).Name)
                                          .Build();
                }
                else
                {
                    jobDetail = JobBuilder.Create<T>()
                                          .WithIdentity(StringHelper.GenerateJobId(jobId) + typeof(T).Name, typeof(T).Name)
                                          .UsingJobData(new JobDataMap(data))
                                          .Build();
                }

                ITrigger trigger = TriggerBuilder.Create()
                                                 .WithIdentity(StringHelper.GenerateJobTriggerId(jobId) + typeof(T).Name, typeof(T).Name)
                                                 .StartNow()
                                                 .Build();

                await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
                await scheduler.Start(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task ExecuteWithCronSchedule<T>(string cronExpression, string jobId, IDictionary<string, object> data = null, CancellationToken cancellationToken = default) where T : IJob
        {
            try
            {
                _logger.LogInformation($"Start to execute job {jobId}-{typeof(T).Name} now");
                IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                IJobDetail jobDetail;

                if (data is null)
                {
                    jobDetail = JobBuilder.Create<T>()
                                          .WithIdentity(StringHelper.GenerateJobId(jobId) + typeof(T).Name, typeof(T).Name)
                                          .Build();
                }
                else
                {
                    jobDetail = JobBuilder.Create<T>()
                                          .WithIdentity(StringHelper.GenerateJobId(jobId) + typeof(T).Name, typeof(T).Name)
                                          .UsingJobData(new JobDataMap(data))
                                          .Build();
                }

                ITrigger trigger = TriggerBuilder.Create()
                                                 .WithIdentity(StringHelper.GenerateJobTriggerId(jobId) + typeof(T).Name, typeof(T).Name)
                                                 .WithCronSchedule(cronExpression)
                                                 .Build();

                await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
                await scheduler.Start(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
