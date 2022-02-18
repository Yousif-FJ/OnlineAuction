using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionBackend.API.RemoteSchema.V1.Authentication
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; init; } = null!;
        [Required]
        public string Password { get; init; } = null!;
    }

    public record LoginResponse(string AccessToken);

}
