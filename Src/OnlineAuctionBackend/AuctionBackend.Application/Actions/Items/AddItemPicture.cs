﻿using AuctionBackend.Application.Actions.Helper;
using AuctionBackend.Application.Database;
using AuctionBackend.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuctionBackend.Application.Actions.Items
{
    public record AddItemPictureCommand(int ItemId, IFormFile File) : IRequest<string>;
    public class AddItemPictureValidator : AbstractValidator<AddItemPictureCommand>
    {
        public AddItemPictureValidator(AuctionDbContext dbContext, IAuctionUserManager userManager)
        {
            RuleFor(request => request.ItemId)
                .Custom((itemId,context) => {
                    var item = dbContext.Items
                        .Find(new object?[] { itemId });

                    if (item is null)
                        context.AddFailure("Item doesn't exist");
                    });

            RuleFor(request => request.ItemId)
                .Custom((userId,context) => {
                    var user = userManager.GetOrCreateAsync().GetAwaiter().GetResult();
                    dbContext.ValidateItemExistAsync(context,
                        context.InstanceToValidate.ItemId).GetAwaiter().GetResult();
                    dbContext.ValidateItemOwnerAsync(context,
                        context.InstanceToValidate.ItemId, user).GetAwaiter().GetResult();
                });
        }
    }

    public class AddItemPictureHandler : IRequestHandler<AddItemPictureCommand, string>
    {
        private readonly AuctionDbContext dbContext;
        private readonly IPictureProvider pictureProvider;

        public AddItemPictureHandler(AuctionDbContext dbContext, IPictureProvider pictureProvider)
        {
            this.dbContext = dbContext;
            this.pictureProvider = pictureProvider;
        }
        public async Task<string> Handle(AddItemPictureCommand request, CancellationToken ct)
        {
            var result = await pictureProvider.SavePicture(
                request.File.OpenReadStream());

            var item = await dbContext.Items
                .FindAsync(new object?[] { request.ItemId }, ct);

            if (item!.PhotoId is not null)
            {
                await pictureProvider.DeletePictureAsync(item.PhotoId);
            }

            item!.PhotoUrl = result.Url;
            item!.PhotoId = result.PublicId;

            await dbContext.SaveChangesAsync(ct);

            return result.Url;
        }
    }
}
