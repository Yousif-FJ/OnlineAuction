using X.PagedList;

namespace AuctionBackend.Api.RemoteSchema.V1
{
    public record PagedResponse<TPagedList>(TPagedList Data) where TPagedList : IPagedList
    {
        public int TotalCount { get; } = Data.TotalItemCount;
    };
}
