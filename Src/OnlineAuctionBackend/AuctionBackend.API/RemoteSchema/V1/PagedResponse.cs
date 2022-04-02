using X.PagedList;

namespace AuctionBackend.Api.RemoteSchema.V1
{
    public record PagedResponse<TPagedList>(TPagedList Date) where TPagedList : IPagedList
    {
        public int TotalCount { get; } = Date.TotalItemCount;
    };
}
