using AuctionBackend.Application.Actions.Helper;
using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using MediatR;
using System.Diagnostics;

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
            Debug.Assert(user is not null,"User can not be null with correct authorization");

            return context.Items
                .OrderBy(x => x.Id)
                .Where(i => i.OwnerId == user.Id);
        }

    }
}
