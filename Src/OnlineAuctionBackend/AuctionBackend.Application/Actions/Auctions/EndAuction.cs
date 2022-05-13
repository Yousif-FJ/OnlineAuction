using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record EndAuctionCommand(int AuctionId) : IRequest<Auction>;
    public class EndAuctionValidator : AbstractValidator<EndAuctionCommand>
    {
        public EndAuctionValidator(IAuctionUserManager userManager, AuctionDbContext auctionDb)
        {
            RuleFor(x => x.AuctionId).NotEmpty();
            RuleFor(x => x.AuctionId).Custom(
                (auctionId, context) =>
            {
                var auction = auctionDb.Auctions.Find(auctionId);
                if (auction == null)
                {
                    context.AddFailure("Auction doesn't exist");
                    return;
                }

                if (auction.HasEnded == false)
                {
                    context.AddFailure("Auction hasn't ended yet");
                    return;
                }

                var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();

                Debug.Assert(user is not null,
                    "User can not be null with correct authorization");

                auctionDb.Entry(auction!).Collection(a => a.Bids)
                    .Load();

                var topBidderUserId = auction!.Bids.OrderByDescending(a => a.Value)
                    .Select(b => b.UserId)
                    .FirstOrDefault();

                if (topBidderUserId == default)
                {
                    context.AddFailure("Auction has no bidder");
                    return;
                }

                if (topBidderUserId != user.Id)
                {
                    context.AddFailure("User is not top bidder");
                    return;
                }
            });
        }
    }

    public class EndAuctionHandler : IRequestHandler<EndAuctionCommand, Auction>
    {
        private readonly IAuctionUserManager userManager;
        private readonly AuctionDbContext auctionDb;

        public EndAuctionHandler(IAuctionUserManager userManager, AuctionDbContext auctionDb)
        {
            this.userManager = userManager;
            this.auctionDb = auctionDb;
        }
        public async Task<Auction> Handle(EndAuctionCommand request, CancellationToken ct)
        {
            var user = await userManager.GetOrCreateAsync();
            var auction = await auctionDb.Auctions.FindAsync(new object?[] { request.AuctionId },
                ct);

            await auctionDb.Entry(auction!).Collection(a => a.Bids)
                .LoadAsync(ct);
            var topBid = auction!.Bids.OrderByDescending(a => a.Value)
                .FirstOrDefault();

            await auctionDb.Entry(auction)
                .Reference(a => a.Item)
                .LoadAsync(ct);

            auction.Item.OwnerId = topBid!.UserId;
            auction.Item.StartingPrice = topBid!.Value;

            await auctionDb.SaveChangesAsync(ct);


            auctionDb.Auctions.Remove(auction);
            await auctionDb.SaveChangesAsync(ct);

            return auction;
        }
    }
}
