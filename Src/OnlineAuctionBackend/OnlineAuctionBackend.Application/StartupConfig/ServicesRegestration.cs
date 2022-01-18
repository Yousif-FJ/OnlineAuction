using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineAuctionBackend.Application.StartupConfig
{
    public static class ServicesRegestration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServicesRegestration));
            services.AddFluentValidation(new[] { typeof(ServicesRegestration).Assembly });
        }
    }
}
