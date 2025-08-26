using System.Net;
using Account_Service.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Exceptions
    // ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class GlobalExceptionHandler : IExceptionHandler
    {
        /// <inheritdoc />
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            MbResult<string> mbResult;

            if (exception is not ValidationException fluentException)
            {
                if (exception is DbUpdateConcurrencyException)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    mbResult = new MbResult<string>(status: (HttpStatusCode)httpContext.Response.StatusCode)
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
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                List<string> validationErrors = [];
                foreach (var error in fluentException.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);

                    if (error.ErrorMessage.Equals("С замороженного счёта нельзя снимать деньги"))
                        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                }

                mbResult = new MbResult<string>(status: (HttpStatusCode)httpContext.Response.StatusCode)
                {
                    MbError = validationErrors
                };
            }

            await httpContext.Response.WriteAsJsonAsync(mbResult, cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}