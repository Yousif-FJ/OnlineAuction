using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineAuctionBackend.Identity.Services;

namespace OnlineAuctionBackend.Identity.Actions
{
    public record CreateUserRequest(string Username, string Email, string Phonenumber, string Password)
    : IRequest<CreateUserResponse>;


    public record CreateUserResponse(string? Error = null, string? AccessToken = null);


    internal class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccessTokenGenerator _tokenGenerator;

        public CreateUserHandler(UserManager<AppUser> userManager, IAccessTokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<CreateUserResponse> Handle(CreateUserRequest request,
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

                return new CreateUserResponse(error);
            }

            var accessToken = _tokenGenerator.GenerateAccessToken(user.UserName, user.Email
                , user.Id);

            return new CreateUserResponse(AccessToken: accessToken);
        }
    }
}
