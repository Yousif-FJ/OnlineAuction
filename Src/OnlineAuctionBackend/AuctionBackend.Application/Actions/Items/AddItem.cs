using AuctionBackend.Application.Actions.Helper;
using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Items
{
    public record AddItemCommand(Item Item, string identityUserId) : IRequest<Item>;

    public class AddItemValidator : AbstractValidator<AddItemCommand>
    {
        public AddItemValidator()
        {
            RuleFor(request => request.Item.StartingPrice).GreaterThan(0);
        }
    }

    public class AddItemHandler : IRequestHandler<AddItemCommand, Item>
    {
        private readonly AuctionDbContext dbContext;

        public AddItemHandler(AuctionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Item> Handle(AddItemCommand request, CancellationToken ct)
        {
            var user = await AuctionUserTool.GetOrCreateAuctionUserAsync(dbContext,
                request.identityUserId);
            request.Item.Owner = user;

            await dbContext.Items.AddAsync(request.Item, ct);
            await dbContext.SaveChangesAsync(ct);

            return request.Item;
        }
    }
}
