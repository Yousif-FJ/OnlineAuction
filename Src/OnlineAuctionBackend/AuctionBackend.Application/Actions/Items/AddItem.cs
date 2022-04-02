using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Items
{
    public record AddItemCommand(Item Item) : IRequest<Item>;

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
        private readonly IAuctionUserManager userManager;

        public AddItemHandler(AuctionDbContext dbContext, IAuctionUserManager userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public async Task<Item> Handle(AddItemCommand request, CancellationToken ct)
        {
            var user = await userManager.GetOrCreateAsync();
            request.Item.Owner = user;

            await dbContext.Items.AddAsync(request.Item, ct);
            await dbContext.SaveChangesAsync(ct);

            return request.Item;
        }
    }
}
