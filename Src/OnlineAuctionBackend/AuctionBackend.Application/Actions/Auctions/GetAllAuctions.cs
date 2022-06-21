using AuctionBackend.Application.Helper;
using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record GetAllAuctionsQuery(string? Search, bool SearchDescription = false,
        AuctionFilterQuery AuctionFilterType = AuctionFilterQuery.Normal) : IRequest<IQueryable<Auction>>;
    public enum AuctionFilterQuery
    {
        Normal,
        MyAuction,
        MyBidAuction,
    }

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
                .AsNoTracking();

            if (request.Search is not null)
            {
                if (request.SearchDescription)
                {
                    query = query.Where(a => a.Item.Name.ToLower()
                        .Contains(request.Search.ToLower()) ||
                                a.Item.Description!.ToLower().Contains(request.Search.ToLower()));
                }
                else
                {
                    query = query.Where(a => a.Item.Name.ToLower()
                        .Contains(request.Search.ToLower()));
                }
            }

            switch (request.AuctionFilterType)
            {
                case AuctionFilterQuery.Normal:
                    {
                        query = query.Where(Auction.HasEndedExpression.Inverse());
                    }
                    break;
                case AuctionFilterQuery.MyAuction:
                    {
                        var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();
                        if (user is null)
                        {
                            throw new ValidationException("User not logged in");
                        }
                        query = query.Where(a => a.Item.OwnerId == user.Id);
                    }
                    break;
                case AuctionFilterQuery.MyBidAuction:
                    {
                        var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();
                        if (user is null)
                        {
                            throw new ValidationException("User not logged in");
                        }
                        query = query.Where(a =>
                                a.Bids.Any(b => b.UserId == user.Id));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return query;
        }
    }
}
