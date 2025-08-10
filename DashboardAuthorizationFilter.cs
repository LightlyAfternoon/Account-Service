using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Account_Service
{
    /// <inheritdoc />
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <inheritdoc />
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.User.Identity != null)
            {
                return httpContext.User.Identity.IsAuthenticated;
            }

            return false;
        }
    }
}