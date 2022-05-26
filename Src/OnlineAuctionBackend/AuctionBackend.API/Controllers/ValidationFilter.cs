using AuctionBackend.Api.RemoteSchema;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuctionBackend.Api.Controllers
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                var errorMessage = exception.Errors.FirstOrDefault()?.ErrorMessage;

                if (errorMessage is null)
                {
                    errorMessage = exception.Message;
                }

                context.Result = new BadRequestObjectResult(
                    new ErrorResponse(errorMessage));
                context.ExceptionHandled = true;
            }
        }
    }
}
