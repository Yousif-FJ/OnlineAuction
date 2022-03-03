using AuctionBackend.Api.Controllers.ControllerFilter;
using AuctionBackend.Api.RemoteSchema.V1;
using AuctionBackend.Api.RemoteSchema.V1.Item;
using AuctionBackend.Application.Actions.Items;
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
    public class ItemsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public ItemsController(IMapper mapper, IMediator mediator)
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }
        [HttpGet(Manifest.GetAllItem)]
        public async Task<IActionResult> GetAllItems([FromQuery]PageInfoRequest pageInfo)
        {
            var result = await mediator.Send(new GetAllItemsQuery());
            var response = await result
                .ProjectTo<ItemRemote>(mapper.ConfigurationProvider)
                .ToPagedListAsync(pageInfo.PageNumber, pageInfo.PageSize);
            return Ok(response);
        }

        [HttpPost(Manifest.PostItem)]
        public async Task<IActionResult> PostItem(CreateItemRequest request)
        {
            var identityUserId = HttpContext.GetUserId();
            var item = mapper.Map<Item>(request);
            var command = new AddItemCommand(item, identityUserId);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<ItemRemote>(result));
        }
    }
}
