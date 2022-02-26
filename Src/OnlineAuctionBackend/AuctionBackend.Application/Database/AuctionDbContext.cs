using Microsoft.EntityFrameworkCore;
using AuctionBackend.Application.Models;

namespace AuctionBackend.Application.Database
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
        {
        }
        public DbSet<Auction> Auctions => Set<Auction>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Bid> Bids => Set<Bid>();
        public DbSet<User> Users => Set<User>();
    }
}
