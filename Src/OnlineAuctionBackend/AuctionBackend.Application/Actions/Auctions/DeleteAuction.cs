using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;

namespace AuctionBackend.Application.Actions.Auctions
{
    public record DeleteAuctionCommand(int AuctionId) : IRequest<Auction>;
    public class DeleteAuctionValidator : AbstractValidator<DeleteAuctionCommand>
    {
        public DeleteAuctionValidator(AuctionDbContext auctionDb, IAuctionUserManager userManager)  
        {
            RuleFor(request => request.AuctionId).NotEmpty();
            RuleFor(request => request.AuctionId)
                .Custom((auctionId, context) =>
                {
                    var auction = auctionDb.Auctions.Find(auctionId);

                    if (auction is null)
                    {
                        context.AddFailure("Auction doesn't exist");
                        return;
                    }

                    if (auctionDb.Bids.Any(b => b.AuctionId == auctionId))
                    {
                        context.AddFailure("Can not remove auction with bids");
                        return;
                    }

                    var itemOwnerId = auctionDb.Items
                        .Where(i => i.Id == auction.ItemId)
                        .Select(i => i.OwnerId)
                        .FirstOrDefault();

                    var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();
                    if (itemOwnerId != user.Id)
                    {
                        context.AddFailure("User is not auction owner");
                        return;
                    }
                });
        }
    }

    public class DeleteAuctionHandler : IRequestHandler<DeleteAuctionCommand, Auction>
    {
        private readonly AuctionDbContext auctionDb;

        public DeleteAuctionHandler(AuctionDbContext auctionDb)
        {
            this.auctionDb = auctionDb;
        }
        public async Task<Auction> Handle(DeleteAuctionCommand request, CancellationToken ct)
        {
            var auction = await auctionDb.Auctions.FindAsync(new object?[] { request.AuctionId },
                cancellationToken: ct);

            await auctionDb.Entry(auction!).Reference(a => a.Item)
                .LoadAsync(ct);

            auctionDb.Remove(auction!);
            await auctionDb.SaveChangesAsync(ct);

            return auction!;
        }
    }
}
