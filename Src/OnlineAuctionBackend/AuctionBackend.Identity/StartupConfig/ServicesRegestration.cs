using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using AuctionBackend.Identity.Actions.Commands;
using AuctionBackend.Identity.Services;
using AuctionBackend.Identity.Database;

namespace AuctionBackend.Identity.StartupConfig
{
    public static class ServicesRegestration
    {
        public static void AddAppIdentityServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateUserHandler));
            services.AddTransient<IAccessTokenGenerator, AccessTokenGenerator>();
            services.ConfigureIdentity();
        }

        private static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<IdentityDbContext>();

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
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}
