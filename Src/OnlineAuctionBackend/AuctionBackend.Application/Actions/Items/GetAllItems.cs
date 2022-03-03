using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using MediatR;

namespace AuctionBackend.Application.Actions.Items
{
    public record GetAllItemsQuery : IRequest<IQueryable<Item>>;

    public class GetAllItemsHandler : RequestHandler<GetAllItemsQuery, IQueryable<Item>>
    {
        private readonly AuctionDbContext context;

        public GetAllItemsHandler(AuctionDbContext context)
        {
            this.context = context;
        }
        protected override IQueryable<Item> Handle(GetAllItemsQuery request)
        {
            return context.Items;
        }
    }
}
