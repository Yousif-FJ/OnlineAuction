namespace AuctionBackend.Api.RemoteSchema.V1.Auction
{
    public record AuctionRemote(int Id, string Username, double CurrentPrice,
        DateTime ExpireDate, DateTime CreationDate, ItemRemote Item, bool HasEnded,
        BidRemote? HighestBids);
    public record AuctionFilter(string? Search, bool SearchDescription = false,
        AuctionFilterType AuctionFilterType = AuctionFilterType.Normal);
    public enum AuctionFilterType
    {
        Normal,
        MyAuction,
        MyBidAuction,
    }
    public record AuctionDetailRemote(int Id, string Username, double CurrentPrice,
        DateTime ExpireDate, DateTime CreationDate, ItemRemote Item, bool HasEnded,
        IEnumerable<BidRemote> Bids);
    public record CreateAuctionRequest(int ItemId, DateTime ExpireDate);
}