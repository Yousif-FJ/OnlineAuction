using AuctionBackend.Api.RemoteSchema.V1;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuctionBackend.Api.Controllers.ControllerFilter
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var exception = (ValidationException)context.Exception;
                var firstError = exception.Errors.First();

                context.Result = new BadRequestObjectResult(
                    new ErrorResponse(firstError.ErrorMessage));
                context.ExceptionHandled = true;
            }
        }
    }
}
