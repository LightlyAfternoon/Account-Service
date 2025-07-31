using System.Net;
using Account_Service.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

namespace Account_Service.Exceptions
{
    /// <inheritdoc />
    public class GlobalExceptionHandler : IExceptionHandler
    {
        /// <inheritdoc />
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var mbResult = new MbResult<List<string>>();

            if (exception is FluentValidation.ValidationException fluentException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                List<string> validationErrors = [];
                foreach (var error in fluentException.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                mbResult.MbError = validationErrors;
            }

            else
            {
                mbResult.MbError = [exception.Message];
            }

            mbResult.Status = (HttpStatusCode)httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(mbResult, cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}