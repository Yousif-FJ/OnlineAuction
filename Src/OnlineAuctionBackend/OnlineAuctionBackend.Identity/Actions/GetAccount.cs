using MediatR;
using Microsoft.AspNetCore.Identity;

namespace OnlineAuctionBackend.Identity.Actions
{
    public record GetUserRequest(string UserId) : IRequest<AppUser?>;

    internal class GetAccountHandler : IRequestHandler<GetUserRequest, AppUser?>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetAccountHandler(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<AppUser?> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            return await _userManager.FindByIdAsync(request.UserId);
        }
    }
}
