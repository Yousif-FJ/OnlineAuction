namespace AuctionBackend.Api.RemoteSchema.V2.Auction
{
    public record CreateAuctionRequest(int ItemId, long TimeSpanInSeconds);
}