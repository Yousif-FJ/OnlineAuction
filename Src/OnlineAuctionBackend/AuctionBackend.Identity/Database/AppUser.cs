using Microsoft.AspNetCore.Identity;

namespace AuctionBackend.Identity.Data
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
