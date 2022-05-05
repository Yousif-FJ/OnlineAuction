using AuctionBackend.Api.RemoteSchema.V1;
using AuctionBackend.Api.RemoteSchema.V1.Auction;
using AuctionBackend.Application.Actions.Auctions;
using AuctionBackend.Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AuctionBackend.Api.Controllers.V1
{
    [ApiController]
    [TypeFilter(typeof(ValidationExceptionFilter))]
    public class AuctionController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public AuctionController(IMapper mapper, IMediator mediator)
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }
        [HttpGet(Manifest.GetAllAuctions)]
        [ProducesResponseType(typeof(PagedResponse<IPagedList<AuctionRemote>>), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAuctions([FromQuery]PageInfoRequest pageInfo,
            [FromQuery] AuctionFilter auctionFilter)
        {
            var command = mapper.Map<GetAllAuctionsQuery>(auctionFilter);
            var result = await mediator.Send(command);
            IPagedList<AuctionRemote>? response = await result
                .ProjectTo<AuctionRemote>(mapper.ConfigurationProvider)
                .ToPagedListAsync(pageInfo.PageNumber, pageInfo.PageSize);
            return Ok(new PagedResponse<IPagedList<AuctionRemote>>(response));
        }

        [HttpPost(Manifest.PostAuction)]
        [ProducesResponseType(typeof(AuctionRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> PostAuction(CreateAuctionRequest request)
        {
            var item = mapper.Map<Auction>(request);
            var command = new AddAuctionCommand(item);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<AuctionRemote>(result));
        }

        [HttpPost(Manifest.PostBid)]
        [ProducesResponseType(typeof(BidRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> PostBid(AddBidRequest request)
        {
            var command = mapper.Map<AddBidCommand>(request);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<BidRemote>(result));
        }

        [HttpPatch(Manifest.EndAuction)]
        [ProducesResponseType(typeof(AuctionRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> EndAuction(int auctionId)
        {
            var command = new EndAuctionCommand(auctionId);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<AuctionRemote>(result));
        }

        [HttpDelete(Manifest.DeleteAuction)]
        [ProducesResponseType(typeof(AuctionRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteAuction(int auctionId)
        {
            var command = new DeleteAuctionCommand(auctionId);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<AuctionRemote>(result));
        }
    }
}
