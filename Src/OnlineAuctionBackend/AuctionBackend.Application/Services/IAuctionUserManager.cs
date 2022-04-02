using AuctionBackend.Application.Models;
using FluentValidation;

namespace AuctionBackend.Application.Services
{
    public interface IAuctionUserManager
    {
        public Task<User> GetOrCreateAsync();
    }
}
