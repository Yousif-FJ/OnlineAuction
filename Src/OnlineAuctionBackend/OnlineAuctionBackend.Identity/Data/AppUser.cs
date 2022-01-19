using Microsoft.AspNetCore.Identity;

namespace OnlineAuctionBackend.Identity.Data
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
