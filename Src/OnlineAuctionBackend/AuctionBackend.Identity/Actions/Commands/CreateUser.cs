using MediatR;
using Microsoft.AspNetCore.Identity;
using AuctionBackend.Identity.Services;

namespace AuctionBackend.Identity.Actions.Commands
{
    public record CreateUserCommand(string Username, string Email, string? Phonenumber, string Password)
    : IRequest<OneOf<CreateUserResult, ErrorMessage>>;


    public record CreateUserResult(string AccessToken);


    public class CreateUserHandler : IRequestHandler<CreateUserCommand, OneOf<CreateUserResult, ErrorMessage>>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IAccessTokenGenerator _tokenGenerator;

        public CreateUserHandler(UserManager<AppUser> userManager, IAccessTokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<OneOf<CreateUserResult, ErrorMessage>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = new AppUser(request.Username)
            {
                Email = request.Email,
                PhoneNumber = request.Phonenumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded == false)
            {
                var error = string.Join(',', result.Errors.Select(e => e.Description));

                return new ErrorMessage(error);
            }

            var accessToken = _tokenGenerator.GenerateAccessToken(user.UserName, user.Email
                , user.Id);

            return new CreateUserResult(AccessToken: accessToken);
        }
    }
}
