using AutoMapper;
using OnlineAuctionBackend.API.RemoteSchema.V1;
using OnlineAuctionBackend.API.RequestResponseSchema.V1.Authentication;

namespace OnlineAuctionBackend.API.MapProfile.V1
{
    public class AuthenticationMapPofile : Profile
    {
        public AuthenticationMapPofile()
        {
            CreateMap<LoginRequest, LoginCommand>(MemberList.Source);
            CreateMap<LoginResult, LoginResponse>(MemberList.Destination);
            CreateMap<LoginResult, ErrorResponse>(MemberList.Destination);
        }
    }
}
