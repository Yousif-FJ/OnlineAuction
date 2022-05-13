using AuctionBackend.Application.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Services
{
    public class AuctionUserManager : IAuctionUserManager
    {
        private readonly AuctionDbContext auctionDb;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuctionUserManager(AuctionDbContext auctionDb, IHttpContextAccessor httpContextAccessor)
        {
            this.auctionDb = auctionDb;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<User?> GetOrCreateAsync()
        {
            var IdentityUserId = httpContextAccessor.HttpContext.GetUserId();

            if (IdentityUserId is null)
            {
                return null;
            }

            var user = await auctionDb.Users.FirstOrDefaultAsync(
                u => u.IdentityId == IdentityUserId);

            if (user == null)
            {
                var usernmae = httpContextAccessor.HttpContext.GetUername();
                user = new User(IdentityUserId, usernmae);
                await auctionDb.Users.AddAsync(user);
                await auctionDb.SaveChangesAsync();
            }

            return user;
        }
    }
}
