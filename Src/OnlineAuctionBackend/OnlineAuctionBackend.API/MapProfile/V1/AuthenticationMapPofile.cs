using AutoMapper;
using OnlineAuctionBackend.API.RemoteSchema.V1.Authentication;
using OnlineAuctionBackend.Identity.Actions.Commands;
using OnlineAuctionBackend.Identity.Data;

namespace OnlineAuctionBackend.API.MapProfile.V1
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
