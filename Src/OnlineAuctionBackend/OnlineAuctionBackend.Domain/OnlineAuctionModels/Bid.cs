using OnlineAuctionBackend.Domain.OnlineAuctionModels.Shared;

namespace OnlineAuctionBackend.Domain.OnlineAuctionModels
{
    public class Bid : EntityBase
    {
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
    }
}
