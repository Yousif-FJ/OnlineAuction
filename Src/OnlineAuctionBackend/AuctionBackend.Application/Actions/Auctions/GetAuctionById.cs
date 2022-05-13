using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record GetAuctionByIdQuery(int Id) : IRequest<IQueryable<Auction>>;
    public class GetAuctionByIdHandler : RequestHandler<GetAuctionByIdQuery, IQueryable<Auction>>
    {
        private readonly AuctionDbContext auctionDb;

        public GetAuctionByIdHandler(AuctionDbContext auctionDb)
        {
            this.auctionDb = auctionDb;
        }

        protected override IQueryable<Auction> Handle(GetAuctionByIdQuery request)
        {
            return auctionDb.Auctions
                .AsNoTracking()
                .Where(a => a.Id == request.Id);
        }
    }
}
