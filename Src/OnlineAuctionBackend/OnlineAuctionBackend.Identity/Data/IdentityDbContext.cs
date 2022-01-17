using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineAuctionBackend.Identity.Data
{
    public class IdentityDbContext : IdentityUserContext<IdentityUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options): base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}
