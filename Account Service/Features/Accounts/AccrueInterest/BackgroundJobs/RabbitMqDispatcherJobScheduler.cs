using Account_Service.Features.RabbitMQ;
using Hangfire;

namespace Account_Service.Features.Accounts.AccrueInterest.BackgroundJobs
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitMqDispatcherJobScheduler
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IRecurringJobManager _recurringJobManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rabbitMqService"></param>
        /// <param name="recurringJobManager"></param>
        public RabbitMqDispatcherJobScheduler(IRabbitMqService rabbitMqService, IRecurringJobManager recurringJobManager)
        {
            _rabbitMqService = rabbitMqService;
            _recurringJobManager = recurringJobManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ScheduleJob()
        {
            _recurringJobManager.AddOrUpdate("rabbit-mq-dispatcher",
                () => _rabbitMqService.PublishAllNonProcessedFromOutbox(),
                "*/10 * * * *"); // runs every 10 minutes
        }
    }
}