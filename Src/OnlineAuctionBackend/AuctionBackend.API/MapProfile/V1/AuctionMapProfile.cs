using AuctionBackend.Api.RemoteSchema.V1.Auction;
using AuctionBackend.Application.Actions.Auctions;
using AuctionBackend.Application.Models;
using AutoMapper;

namespace AuctionBackend.Api.MapProfile.V1
{
    public class AuctionMapProfile : Profile
    {
        public AuctionMapProfile()
        {
            CreateMap<Auction, AuctionRemote>(MemberList.Destination)
                .ForCtorParam(nameof(AuctionRemote.HighestBids),
                opt => opt.MapFrom(s => s.Bids.OrderBy(b => b.Value)
                                    .LastOrDefault()))
                .ForCtorParam(nameof(AuctionRemote.Username),
                opt => opt.MapFrom(s => s.Item.Owner.Name));

            CreateMap<Auction, AuctionDetailRemote>(MemberList.Destination)
                .ForCtorParam(nameof(AuctionRemote.Username),
                opt => opt.MapFrom(s => s.Item.Owner.Name));

            CreateMap<AuctionFilter, GetAllAuctionsQuery>(MemberList.Source);
            CreateMap<CreateAuctionRequest, Auction>(MemberList.Source)
                .ForCtorParam(nameof(Auction.CreationDate),
                opt => opt.MapFrom(s => DateTime.UtcNow));
        }
    }
}
