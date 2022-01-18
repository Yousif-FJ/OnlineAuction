using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OnlineAuctionBackend.Infrastructure.Data;

namespace OnlineAuctionBackend.Infrastructure.StartupConfig
{
    public static class DatabaseRegistation
    {
        public static void CustomConfigAuctionDb(this IServiceCollection services, string databaseUrl)
        {
            if (string.IsNullOrEmpty(databaseUrl))
            {
                throw new ArgumentException($"'{nameof(databaseUrl)}' cannot be null or empty.",
                    nameof(databaseUrl));
            }

            services.AddDbContext<AuctionDbContext>(optionsBuilder =>
            {
                var databaseUri = new Uri(databaseUrl);
                var userInfo = databaseUri.UserInfo.Split(':');

                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.LocalPath.TrimStart('/'),
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true
                };

                optionsBuilder.UseNpgsql(builder.ToString());
            });
        }
    }
}
