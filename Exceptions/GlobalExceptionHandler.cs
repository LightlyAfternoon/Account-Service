using Account_Service.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            else if (exception is DbUpdateConcurrencyException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                mbResult.MbError = ["Конфликт версий объекта"];
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