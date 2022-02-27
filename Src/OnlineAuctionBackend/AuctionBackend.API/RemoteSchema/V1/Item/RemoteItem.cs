namespace AuctionBackend.Api.RemoteSchema.V1.Item
{
    public record ItemRemote(int Id, string Name, double StartingPrice, string? PhotoUrl);
    public record CreateItemRequest(string Name, double StartingPrice);

}
