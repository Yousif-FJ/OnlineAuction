using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OnlineAuctionBackend.Infrastructure.Data;

namespace OnlineAuctionBackend.Infrastructure.StartupConfig
{
    public static class DatabaseRegistation
    {
        public static void CustomConfigAuctionDb(this IServiceCollection services,
            IConfiguration Configuration)
        {
            services.AddDbContext<AuctionDbContext>(optionsBuilder =>
            {
                string databaseUrl = Configuration["DATABASE_URL"];
                if (databaseUrl == null)
                {
                    throw new InvalidOperationException("Couldn't get connection string");
                }

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
