using AuctionBackend.Api.RemoteSchema;
using AuctionBackend.Application.Actions.Auctions;
using AuctionBackend.Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionBackend.Api.Controllers.V2
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

        [HttpPost(Manifest.PostAuction)]
        [ProducesResponseType(typeof(RemoteSchema.V1.Auction.AuctionRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> PostAuction(RemoteSchema.V2.Auction.CreateAuctionRequest request)
        {
            var item = mapper.Map<Auction>(request);
            var command = new AddAuctionCommand(item);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<RemoteSchema.V1.Auction.AuctionRemote>(result));
        }
    }
}