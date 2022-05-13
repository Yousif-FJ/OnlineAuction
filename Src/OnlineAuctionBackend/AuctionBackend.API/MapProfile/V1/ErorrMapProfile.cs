using AutoMapper;
using AuctionBackend.Api.RemoteSchema.V1;
using AuctionBackend.Identity.Actions.HelperObj;

namespace AuctionBackend.Api.MapProfile.V1
{
    public class ErorrMapProfile : Profile
    {
        public ErorrMapProfile()
        {
            CreateMap<ErrorMessage, ErrorResponse>(MemberList.Destination);
        }
    }
}
