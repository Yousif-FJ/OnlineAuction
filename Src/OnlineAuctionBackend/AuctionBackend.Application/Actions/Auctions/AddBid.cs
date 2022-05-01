using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MediatR;
using AuctionBackend.Application.Services;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record AddBidCommand(double Value, int AuctionId): IRequest<Bid>;

    public class AddBidValidator : AbstractValidator<AddBidCommand>
    {
        public AddBidValidator(AuctionDbContext dbContext)
        {
            RuleFor(request => request.Value).NotEmpty();
            RuleFor(request => request.Value)
                .Custom((value, context) =>
                {
                    var auction = dbContext.Auctions.Find(context.InstanceToValidate.AuctionId);

                    if (auction is null)
                    {
                        return;
                    }

                     dbContext.Entry(auction)
                        .Reference(a => a.Item)
                        .Load();

                    if (value < auction.Item.StartingPrice)
                    {
                        context.AddFailure("Bid value can not be less than item value");
                        return;
                    }

                    if (auction.Bids.AsQueryable().Any())
                    {
                        var maxBidValue = auction.Bids.AsQueryable()
                            .Max(b => b.Value);

                        if (value < maxBidValue)
                        {
                            context.AddFailure("Bid value can not be less than other bid values");
                            return;
                        }
                    }
                });

            RuleFor(request => request.AuctionId).NotEmpty();
            RuleFor(request => request.AuctionId)
                .Custom((auctionId, context) =>
                {
                    var auction = dbContext.Auctions.Find(auctionId);
                    if (auction == null)
                    {
                        context.AddFailure("Auction doesn't exist");
                        return;
                    }

                    if (auction.ExpireDate < DateTime.UtcNow)
                    {
                        context.AddFailure("Auction has expired");
                        return;
                    }
                });
        }
    }

    public class AddBidHandler : IRequestHandler<AddBidCommand, Bid>
    {
        private readonly AuctionDbContext dbContext;
        private readonly IAuctionUserManager userManager;

        public AddBidHandler(AuctionDbContext dbContext, IAuctionUserManager userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<Bid> Handle(AddBidCommand request, CancellationToken ct)
        {
            var user = await userManager.GetOrCreateAsync();
            var bid = new Bid(request.Value, DateTime.UtcNow, request.AuctionId,
                user.Id);

            await dbContext.Bids.AddAsync(bid, ct);
            await dbContext.SaveChangesAsync(ct);

            await dbContext.Entry(bid)
                .Reference(b => b.User)
                .LoadAsync(ct);

            return bid;
        }
    }
}
