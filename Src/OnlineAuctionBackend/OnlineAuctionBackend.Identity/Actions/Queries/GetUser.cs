using MediatR;
using Microsoft.AspNetCore.Identity;

namespace OnlineAuctionBackend.Identity.Actions.Queries
{
    public record GetUserQuery(string UserId) : IRequest<AppUser?>;

    public class GetAccountHandler : IRequestHandler<GetUserQuery, AppUser?>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetAccountHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AppUser?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.FindByIdAsync(request.UserId);
        }
    }
}
