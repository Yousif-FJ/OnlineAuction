using System.Security.Claims;

namespace OnlineAuctionBackend.API.Controllers
{
    public static class Extensions
    {
        public static string GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new Exception($"Claim of type {nameof(ClaimTypes.NameIdentifier)} was null");

            return userId;
        }
    }
}
