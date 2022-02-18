using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Api.RemoteSchema.V1.Authentication
{
    public class CreateUserRequest
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

        public string? PhoneNumber { get; set; }
    }
    public record CreateUserResponse(string AccessToken);
}
