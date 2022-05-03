using AuctionBackend.Application.Database;
using AuctionBackend.Application.Helper;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record GetAllAuctionsQuery(bool IsMyAuctions = false) : IRequest<IQueryable<Auction>>;

    public class GetAllAuctionsHandler : RequestHandler<GetAllAuctionsQuery, IQueryable<Auction>>
    {
        private readonly AuctionDbContext context;
        private readonly IAuctionUserManager userManager;

        public GetAllAuctionsHandler(AuctionDbContext context, IAuctionUserManager userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        protected override IQueryable<Auction> Handle(GetAllAuctionsQuery request)
        {
            var query = context.Auctions
                .OrderBy(x => x.Id)
                .Include(x => x.Item)
                .AsNoTracking();

            if (request.IsMyAuctions)
            {
                var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();

                query = query.Where(a => a.Item.OwnerId == user.Id);
            }
            else
            {
                query = query.Where(Auction.HasEndedExpression.Inverse());
            }

            return query;
        }
    }
}
