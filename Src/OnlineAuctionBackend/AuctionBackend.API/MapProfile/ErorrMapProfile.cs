using AuctionBackend.Api.RemoteSchema;
using AuctionBackend.Identity.Actions.HelperObj;
using AutoMapper;

namespace AuctionBackend.Api.MapProfile
{
    public class ErorrMapProfile : Profile
    {
        public ErorrMapProfile()
        {
            CreateMap<ErrorMessage, ErrorResponse>(MemberList.Destination);
        }
    }
}
