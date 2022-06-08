using AuctionBackend.Identity.Actions.HelperObj;
using AuctionBackend.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

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
            if (request.Phonenumber is not null &&
                IsPhoneNbr(request.Phonenumber) == false)
            {
                return new ErrorMessage("Phone number is not valid");
            }

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
        private const string PhoneNumberRegex = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";
        public static bool IsPhoneNbr(string number)
        {
            return Regex.IsMatch(number, PhoneNumberRegex);
        }
    }
}
