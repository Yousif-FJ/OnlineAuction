using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;
using System.Diagnostics;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record AddBidCommand(double Value, int AuctionId) : IRequest<Bid>;

    public class AddBidValidator : AbstractValidator<AddBidCommand>
    {
        public AddBidValidator(AuctionDbContext dbContext, IAuctionUserManager userManager)
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

                    var isThereAnyBid = dbContext.Entry(auction)
                       .Collection(a => a.Bids)
                       .Query()
                       .Any();

                    if (isThereAnyBid)
                    {
                        var maxBidValue = dbContext.Entry(auction)
                            .Collection(a => a.Bids)
                            .Query()
                            .Max(b => b.Value);

                        if (value <= maxBidValue)
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
                    var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();
                    Debug.Assert(user is not null,
                        "User can't be null with correct authorization");

                    var auction = dbContext.Auctions.Find(auctionId);
                    if (auction == null)
                    {
                        context.AddFailure("Auction doesn't exist");
                        return;
                    }

                    var auctionUserId = dbContext.Entry(auction)
                                        .Reference(auction => auction.Item)
                                        .Query()
                                        .Select(item => item.OwnerId)
                                        .First();

                    if (user.Id == auctionUserId)
                    {
                        context.AddFailure("Can't bid on your own auction");
                        return;
                    }

                    if (auction.HasEnded)
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

            Debug.Assert(user is not null, "User can not be null with correct authorization");

            var bid = new Bid(request.Value, DateTime.UtcNow, request.AuctionId,
                user.Id);

            var auction = dbContext.Auctions.Find(request.AuctionId);
            auction!.CurrentPrice = bid.Value; 

            await dbContext.Bids.AddAsync(bid, ct);
            await dbContext.SaveChangesAsync(ct);

            await dbContext.Entry(bid)
                .Reference(b => b.User)
                .LoadAsync(ct);

            return bid;
        }
    }
}
