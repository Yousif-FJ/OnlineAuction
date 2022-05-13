using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Identity.Database
{
    public class IdentityDbContext : IdentityUserContext<AppUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }
    }
}
