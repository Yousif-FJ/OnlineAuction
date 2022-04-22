﻿using AuctionBackend.Application.Actions.Helper;
using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Application.Actions.Items
{
    public record RemoveItemCommand(int Id) : IRequest<Item>;
    public class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
    {
        public RemoveItemValidator(AuctionDbContext dbContext, IAuctionUserManager userManager)
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Id).Custom((Id, context) =>
            {
                var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();

                dbContext.ValidateItemExistAsync(context,
                    Id).GetAwaiter().GetResult();

                dbContext.ValidateItemOwnerAsync(context,
                    Id, user).GetAwaiter().GetResult();

                var item = dbContext.Items.Include(i => i.Auction)
                        .FirstOrDefault(i => i.Id == Id);

                if (item?.Auction is not null)
                {
                    context.AddFailure("Item is in an auction");
                }
            });
        }
    }
    public class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Item>
    {
        private readonly AuctionDbContext dbContext;

        public RemoveItemHandler(AuctionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Item> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
        {
            var item = await dbContext.Items.FindAsync(new object?[] { request.Id },
                cancellationToken: cancellationToken);
            dbContext.Remove(item!);
            await dbContext.SaveChangesAsync(cancellationToken);

            return item!;
        }
    }
}
