using AuctionBackend.Application.Helper;
using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record AddAuctionCommand(Auction Auction) : IRequest<Auction>;

    public class AddAuctionValidator : AbstractValidator<AddAuctionCommand>
    {
        public AddAuctionValidator(IAuctionUserManager userManager, AuctionDbContext dbContext)
        {
            RuleFor(request => request.Auction.ExpireDate)
                .GreaterThan(DateTime.UtcNow);

            RuleFor(request => request.Auction.ItemId)
                .Custom((ItemId, context) =>
                {
                    var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();

                    var doesExist = dbContext.ValidateItemExistAsync(context,
                        ItemId).GetAwaiter().GetResult();

                    if (!doesExist)
                        return;

                    var item = dbContext.Items.Find(ItemId);

                    dbContext.Entry(item!).Reference(i => i.Auction)
                        .Load();

                    if (item!.Auction is not null)
                    {
                        context.AddFailure("Item is already in an acution");
                        return;
                    }
                });
        }
    }

    public class AddAuctionHandler : IRequestHandler<AddAuctionCommand, Auction>
    {
        private readonly AuctionDbContext dbContext;

        public AddAuctionHandler(AuctionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Auction> Handle(AddAuctionCommand request, CancellationToken ct)
        {
            var item = await dbContext.Items
                .FindAsync(new object?[] { request.Auction.ItemId }, ct);

            request.Auction.CurrentPrice = item!.StartingPrice;
            await dbContext.Auctions.AddAsync(request.Auction, ct);
            await dbContext.SaveChangesAsync(ct);

            return request.Auction;
        }
    }
}
