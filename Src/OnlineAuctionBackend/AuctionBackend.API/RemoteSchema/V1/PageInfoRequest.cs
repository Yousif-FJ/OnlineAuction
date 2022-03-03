namespace AuctionBackend.Api.RemoteSchema.V1
{
    public record PageInfoRequest(int PageNumber = 1, int PageSize = 20);
}
