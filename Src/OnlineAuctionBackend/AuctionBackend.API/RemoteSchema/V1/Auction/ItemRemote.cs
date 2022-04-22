namespace AuctionBackend.Api.RemoteSchema.V1.Item
{
    public record ItemRemote(int Id, string Name, string? Description, double StartingPrice, string? PhotoUrl);
    public record CreateItemRequest(string Name, string? Description, double StartingPrice);
    public record AddItemPhotoRequest(int Id, IFormFile Photo);
    public record DeleteItemRequest(int Id);
}
