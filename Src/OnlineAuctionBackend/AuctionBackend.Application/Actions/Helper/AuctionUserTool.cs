using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Helper
{
    public static class AuctionUserTool
    {
        public static async Task<User> GetOrCreateAuctionUserAsync(AuctionDbContext dbContext,
            string IdentityUserId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(
                u => u.IdentityId == IdentityUserId);

            if (user == null)
            {
                user = new User(IdentityUserId);
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }

            return user;
        }
    }
}
