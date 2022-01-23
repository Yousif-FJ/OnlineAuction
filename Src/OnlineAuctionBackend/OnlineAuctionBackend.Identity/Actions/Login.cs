using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineAuctionBackend.Identity.Services;

namespace OnlineAuctionBackend.Identity.Actions
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;


    public record LoginResult(string? Error = null, string? AccessToken = null);


    public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public LoginHandler(UserManager<AppUser> userManager, IAccessTokenGenerator accessTokenGenerator)
        {
            this._userManager = userManager;
            this._accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginResult("Invalid login credential");
            }

            var success = await _userManager.CheckPasswordAsync(user, request.Password);
            if (success == false)
            {
                return new LoginResult("Invalid login credential");
            }

            var accesToken = _accessTokenGenerator.GenerateAccessToken(user.UserName, user.Email,
                user.Id);

            return new LoginResult(AccessToken: accesToken);
        }
    }
}
