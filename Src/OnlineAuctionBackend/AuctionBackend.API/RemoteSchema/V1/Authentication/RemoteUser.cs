namespace AuctionBackend.Api.RemoteSchema.V1.Authentication
{
    public record RemoteUser(string UserName, string Email, string? PhoneNumber);
}
