using MediatR;
using Microsoft.AspNetCore.Identity;
using AuctionBackend.Identity.Services;

namespace AuctionBackend.Identity.Actions.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<OneOf<LoginResult, ErrorMessage>>;


    public record LoginResult(string AccessToken);


    public class LoginHandler : IRequestHandler<LoginCommand, OneOf<LoginResult, ErrorMessage>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public LoginHandler(UserManager<AppUser> userManager, IAccessTokenGenerator accessTokenGenerator)
        {
            _userManager = userManager;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<OneOf<LoginResult, ErrorMessage>> Handle(LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ErrorMessage("Invalid login credential");
            }

            var success = await _userManager.CheckPasswordAsync(user, request.Password);
            if (success == false)
            {
                return new ErrorMessage("Invalid login credential");
            }

            var accesToken = _accessTokenGenerator.GenerateAccessToken(user.UserName, user.Email,
                user.Id);

            return new LoginResult(AccessToken: accesToken);
        }
    }
}
