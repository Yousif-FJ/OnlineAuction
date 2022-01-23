using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionBackend.API.RequestResponseSchema.V1.Authentication
{
    public class LoginRequest
    {
        [Required]
        public string? Email { get; init; }
        [Required]
        public string? Password { get; init; }
    }

    public record LoginResponse(string AccessToken);

}
