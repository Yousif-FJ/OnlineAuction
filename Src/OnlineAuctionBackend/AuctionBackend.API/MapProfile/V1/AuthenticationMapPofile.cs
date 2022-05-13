using AuctionBackend.Api.RemoteSchema.V1.Authentication;
using AuctionBackend.Identity.Actions.Commands;
using AuctionBackend.Identity.Database;
using AutoMapper;

namespace AuctionBackend.Api.MapProfile.V1
{
    public class AuthenticationMapPofile : Profile
    {
        public AuthenticationMapPofile()
        {
            CreateMap<LoginRequest, LoginCommand>(MemberList.Source);
            CreateMap<LoginResult, LoginResponse>(MemberList.Destination);

            CreateMap<CreateUserRequest, CreateUserCommand>(MemberList.Source);
            CreateMap<CreateUserResult, CreateUserResponse>(MemberList.Destination);

            CreateMap<AppUser, RemoteUser>(MemberList.Destination);
        }
    }
}
