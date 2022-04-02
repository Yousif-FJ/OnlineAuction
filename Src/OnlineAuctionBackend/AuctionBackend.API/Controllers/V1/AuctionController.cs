﻿using AuctionBackend.Api.Controllers.ControllerFilter;
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
        [ProducesResponseType(typeof(AuctionRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAuctions([FromQuery]PageInfoRequest pageInfo)
        {
            var result = await mediator.Send(new GetAllAuctionsQuery());
            IPagedList<AuctionRemote>? response = await result
                .ProjectTo<AuctionRemote>(mapper.ConfigurationProvider)
                .ToPagedListAsync(pageInfo.PageNumber, pageInfo.PageSize);
            return Ok(new PagedResponse<IPagedList<AuctionRemote>>(response));
        }

        [HttpPost(Manifest.PostAuction)]
        [ProducesResponseType(typeof(AuctionRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> PostItem(CreateAuctionRequest request)
        {
            var item = mapper.Map<Auction>(request);
            var command = new AddAuctionCommand(item);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<AuctionRemote>(result));
        }
    }
}
