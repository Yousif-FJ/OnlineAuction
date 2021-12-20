using OnlineAuctionBackend.Domain.OnlineAuctionModels.Shared;

namespace OnlineAuctionBackend.Domain.OnlineAuctionModels
{
    public class Auction: EntityBase
    {
        public double CurrentPrice { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public Item? Item { get; set; }
        public int ItemId { get; set; } 
    }
}
