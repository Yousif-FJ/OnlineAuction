using AuctionBackend.Api.RemoteSchema.V1.Auction;
using AuctionBackend.Application.Actions.Auctions;
using AuctionBackend.Application.Models;
using AutoMapper;

namespace AuctionBackend.Api.MapProfile.V1
{
    public class BidMapProfile : Profile
    {
        public BidMapProfile()
        {
            CreateMap<Bid, BidRemote>(MemberList.Destination)
                .ForCtorParam(nameof(BidRemote.Username),
                opt => opt.MapFrom(s => s.User.Name));

            CreateMap<AddBidRequest, AddBidCommand>(MemberList.Source);
        }
    }
}
