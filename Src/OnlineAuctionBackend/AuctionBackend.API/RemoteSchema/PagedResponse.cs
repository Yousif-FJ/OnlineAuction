using X.PagedList;

namespace AuctionBackend.Api.RemoteSchema
{
    public record PagedResponse<TPagedList>(TPagedList Data) where TPagedList : IPagedList
    {
        public int TotalCount { get; } = Data.TotalItemCount;
    };
}
