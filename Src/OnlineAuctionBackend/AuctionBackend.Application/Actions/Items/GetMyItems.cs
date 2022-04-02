using AuctionBackend.Application.Actions.Helper;
using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using MediatR;

namespace AuctionBackend.Application.Actions.Items
{
    public record GetMyItemsQuery() : IRequest<IQueryable<Item>>;

    public class GetAllItemsHandler : IRequestHandler<GetMyItemsQuery, IQueryable<Item>>
    {
        private readonly AuctionDbContext context;
        private readonly IAuctionUserManager userManager;

        public GetAllItemsHandler(AuctionDbContext context, IAuctionUserManager userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IQueryable<Item>> Handle(GetMyItemsQuery request, CancellationToken ct)
        {
            var user = await userManager.GetOrCreateAsync();
                
            return context.Items
                .OrderBy(x => x.Id)
                .Where(i => i.OwnerId == user.Id);
        }

    }
}
