using Hangfire;

namespace Account_Service.Features.Accounts.AccrueInterest.BackgroundJobs
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    public class DailyAccrueInterestJobScheduler
    {
        private readonly IAccountsService _accountService;
        private readonly IRecurringJobManager _recurringJobManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="recurringJobManager"></param>
        public DailyAccrueInterestJobScheduler(IAccountsService accountService, IRecurringJobManager recurringJobManager)
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