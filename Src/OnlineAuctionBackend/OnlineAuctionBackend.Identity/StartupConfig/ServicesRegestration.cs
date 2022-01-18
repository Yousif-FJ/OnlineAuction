using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionBackend.Identity.Data;

namespace OnlineAuctionBackend.Identity.StartupConfig
{
    public static class ServicesRegestration
    {
        public static void AddAppIdentityServices(this IServiceCollection services)
        {
            services.ConfigureIdentity();
            services.AddMediatR(typeof(ServicesRegestration));
            services.AddFluentValidation(new[] { typeof(ServicesRegestration).Assembly });
        }


        private static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<IdentityDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "QWERTYUIOPASDFGHJKLZXCVBNMabcdefghijklmnopqrstuvwxyz0123456789_";
            });
        }
    }
}
