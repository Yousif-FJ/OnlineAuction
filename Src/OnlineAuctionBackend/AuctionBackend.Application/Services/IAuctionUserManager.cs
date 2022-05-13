namespace AuctionBackend.Application.Services
{
    public interface IAuctionUserManager
    {
        public Task<User?> GetOrCreateAsync();
    }
}
