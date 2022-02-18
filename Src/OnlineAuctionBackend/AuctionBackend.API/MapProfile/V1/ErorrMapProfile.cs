using AutoMapper;
using AuctionBackend.Api.RemoteSchema.V1;

namespace AuctionBackend.Api.MapProfile.V1
{
    public class ErorrMapProfile : Profile
    {
        public ErorrMapProfile()
        {
            CreateMap<Identity.Data.ErrorMessage, ErrorResponse>(MemberList.Destination);
        }
    }
}
