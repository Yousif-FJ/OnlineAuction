using AuctionBackend.Api.RemoteSchema.V1.Item;

namespace AuctionBackend.Api.RemoteSchema.V1.Auction
{
    public record AuctionRemote(int Id, string Username, double CurrentPrice,
        DateTime ExpireDate, DateTime CreationDate, ItemRemote Item, bool HasEnded,
        IEnumerable<BidRemote> Bids);
    public record CreateAuctionRequest(int ItemId, DateTime ExpireDate);
}