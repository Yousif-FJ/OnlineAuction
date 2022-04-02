using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record GetAllAuctionsQuery() : IRequest<IQueryable<Auction>>;

    public class GetAllAuctionsHandler : RequestHandler<GetAllAuctionsQuery, IQueryable<Auction>>
    {
        private readonly AuctionDbContext context;

        public GetAllAuctionsHandler(AuctionDbContext context)
        {
            this.context = context;
        }

        protected override IQueryable<Auction> Handle(GetAllAuctionsQuery request)
        {
            return context.Auctions
                .OrderBy(x => x.Id)
                .Include(x => x.Item)
                .AsNoTracking();
        }
    }
}
