using System.Net;
using Account_Service.Infrastructure;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Account_Service
    // ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <inheritdoc />
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.User.Identity == null)
                return false;
            if (httpContext.User.Identity.IsAuthenticated) 
                return httpContext.User.Identity.IsAuthenticated;

            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            httpContext.Response.WriteAsJsonAsync(new MbResult<string>(status: HttpStatusCode.Unauthorized)
            {
                MbError = ["error: no Authorization header with JWT token or token isn't valid"]
            }).Wait();

            return httpContext.User.Identity.IsAuthenticated;

        }
    }
}