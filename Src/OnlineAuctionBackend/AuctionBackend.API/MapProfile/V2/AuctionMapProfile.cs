using AuctionBackend.Api.RemoteSchema.V2.Auction;
using AuctionBackend.Application.Models;
using AutoMapper;

namespace AuctionBackend.Api.MapProfile.V2
{
    public class AuctionMapProfile : Profile
    {
        public AuctionMapProfile()
        {
            CreateMap<CreateAuctionRequest, Auction>(MemberList.Source)
                .ForCtorParam(nameof(Auction.ExpireDate),
                opt => opt.MapFrom(s => DateTime.UtcNow
                                        .AddSeconds(s.TimeSpanInSeconds)))
                .ForCtorParam(nameof(Auction.CreationDate),
                opt => opt.MapFrom(s => DateTime.UtcNow));
        }
    }
}
