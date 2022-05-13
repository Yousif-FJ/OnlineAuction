using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace AuctionBackend.Identity.StartupConfig
{
    public static class DatabaseRegistration
    {
        public static void CustomConfigIdentityDb(this IServiceCollection services, string databaseUrl)
        {
            services.AddDbContext<IdentityDbContext>(optionsBuilder =>
            {
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
