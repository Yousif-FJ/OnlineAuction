using AuctionBackend.Application.Database;
using AuctionBackend.Application.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuctionBackend.Application.Helper
{
    public static class AuctionTools
    {
        public static string? GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }

        public static string GetUername(this HttpContext context)
        {
            var userId = context.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            if (userId == null)
                throw new Exception($"Claim of type {nameof(ClaimTypes.Name)} was null");

            return userId;
        }

        public static async Task<bool> ValidateItemExistAsync<T>(this AuctionDbContext auctionDb,
            ValidationContext<T> ValidationContext, int itemId)
        {
            var item = await auctionDb.Items
                .FindAsync(new object?[] { itemId });

            if (item is null)
            {
                ValidationContext.AddFailure("Item doesn't exist");
                return false;
            }
            return true;
        }

        public static async Task ValidateItemOwnerAsync<T>(this AuctionDbContext auctionDb,
            ValidationContext<T> ValidationContext, int itemId, User user)
        {
            var item = await auctionDb.Items
                .FindAsync(new object?[] { itemId });

            if (item?.OwnerId != user.Id)
            {
                ValidationContext.AddFailure("Item is not owned by this user");
            }
        }
    }
}
