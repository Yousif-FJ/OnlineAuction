using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MediatR;
using AuctionBackend.Application.Actions.Helper;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record AddAuctionCommand(Auction Auction) : IRequest<Auction>;

    public class AddItemValidator : AbstractValidator<AddAuctionCommand>
    {
        public AddItemValidator(IAuctionUserManager userManager, AuctionDbContext dbContext)
        {
            RuleFor(request => request.Auction.ExpireDate).GreaterThan(DateTime.Now);
            RuleFor(request => request.Auction.ItemId)
                .Custom((userId, context) =>
                {
                    var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();
                    dbContext.ValidateItemExistAsync(context,
                        context.InstanceToValidate.Auction.ItemId).GetAwaiter().GetResult();

                    var item = dbContext.Items.Include(i => i.Auction)
                            .First(i => i.Id == context.InstanceToValidate.Auction.ItemId);

                    if (item.Auction is not null)
                    {
                        context.AddFailure("Item was already in an acution");
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
