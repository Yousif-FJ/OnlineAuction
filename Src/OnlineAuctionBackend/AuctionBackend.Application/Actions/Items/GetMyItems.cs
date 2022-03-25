using AuctionBackend.Application.Actions.Helper;
using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using MediatR;

namespace AuctionBackend.Application.Actions.Items
{
    public record GetMyItemsQuery(string IdentityUserId) : IRequest<IQueryable<Item>>;

    public class GetAllItemsHandler : IRequestHandler<GetMyItemsQuery, IQueryable<Item>>
    {
        private readonly AuctionDbContext context;

        public GetAllItemsHandler(AuctionDbContext context)
        {
            this.context = context;
        }

        public async Task<IQueryable<Item>> Handle(GetMyItemsQuery request, CancellationToken cancellationToken)
        {
            var user = await AuctionUserTool.GetOrCreateAuctionUserAsync(context,
                request.IdentityUserId);

            return context.Items
                .OrderBy(x => x.Id)
                .Where(i => i.OwnerId == user.Id);
        }

    }
}
