using AuctionBackend.Api.RemoteSchema.V1.Auction;
using AuctionBackend.Application.Models;
using AutoMapper;

namespace AuctionBackend.Api.MapProfile.V1
{
    public class ItemMapProfile : Profile
    {
        public ItemMapProfile()
        {
            CreateMap<CreateItemRequest, Item>(MemberList.Source);
            CreateMap<Item, ItemRemote>(MemberList.Destination);
        }
    }
}
