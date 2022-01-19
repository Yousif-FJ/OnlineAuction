using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineAuctionBackend.Identity.Data
{
    internal class IdentityDbContext : IdentityUserContext<AppUser>
    {
        internal IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }
    }
}
