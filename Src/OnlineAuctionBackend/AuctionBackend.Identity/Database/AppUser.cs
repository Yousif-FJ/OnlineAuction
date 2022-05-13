using Microsoft.AspNetCore.Identity;

namespace AuctionBackend.Identity.Database
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
        }

        public AppUser(string userName) : base(userName)
        {
        }
    }
}
