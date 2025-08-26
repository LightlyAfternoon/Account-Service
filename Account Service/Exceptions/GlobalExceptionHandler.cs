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
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            MbResult<string> mbResult;

            if (exception is FluentValidation.ValidationException fluentException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                List<string> validationErrors = [];
                foreach (var error in fluentException.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }

                mbResult = new(status: (HttpStatusCode)httpContext.Response.StatusCode)
                {
                    MbError = validationErrors
                };
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                mbResult = new(status: (HttpStatusCode)httpContext.Response.StatusCode)
                {
                    MbError = ["Конфликт версий объекта"]
                };
            }

            else
            {
                mbResult = new MbResult<string>(status: (HttpStatusCode)httpContext.Response.StatusCode)
                {
                    MbError = [exception.Source + ":" + exception.Message]
                };
            }

            await httpContext.Response.WriteAsJsonAsync(mbResult, cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}