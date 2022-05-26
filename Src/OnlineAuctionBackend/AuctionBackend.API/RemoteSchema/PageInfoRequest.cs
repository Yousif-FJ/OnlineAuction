namespace AuctionBackend.Api.RemoteSchema
{
    public record PageInfoRequest(int PageNumber = 1, int PageSize = 20);
}
