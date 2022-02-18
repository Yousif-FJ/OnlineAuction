using AutoMapper;
using OnlineAuctionBackend.API.RemoteSchema.V1;

namespace OnlineAuctionBackend.API.MapProfile.V1
{
    public class ErorrMapProfile : Profile
    {
        public ErorrMapProfile()
        {
            CreateMap<Identity.Data.ErrorMessage, ErrorResponse>(MemberList.Destination);
        }
    }
}
