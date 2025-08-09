using Hangfire;

namespace Account_Service.Features.Accounts.AccrueInterest.BackgroundJobs
{
    /// <summary>
    /// 
    /// </summary>
    public class DailyAccrueInterestJobScheduler
    {
        private readonly IAccountService _accountService;
        private readonly IRecurringJobManager _recurringJobManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="recurringJobManager"></param>
        public DailyAccrueInterestJobScheduler(IAccountService accountService, IRecurringJobManager recurringJobManager)
        {
            _accountService = accountService;
            _recurringJobManager = recurringJobManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ScheduleJob()
        {
            _recurringJobManager.AddOrUpdate("daily-accrue-interest",
                 () => _accountService.ProcessDailyAccrueInterest(),
                Cron.Daily());
        }
    }
}