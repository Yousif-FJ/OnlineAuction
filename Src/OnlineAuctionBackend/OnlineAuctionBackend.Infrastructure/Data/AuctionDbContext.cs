using Microsoft.EntityFrameworkCore;
using OnlineAuctionBackend.Domain.OnlineAuctionModels;

namespace OnlineAuctionBackend.Infrastructure.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options): base(options)
        {
        }
        public DbSet<Auction>? Auctions { get; set; }
        public DbSet<Item>? Items { get; set; }
        public DbSet<Bid>? Bids { get; set; }
    }
}
