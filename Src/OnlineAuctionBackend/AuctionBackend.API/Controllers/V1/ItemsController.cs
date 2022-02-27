using AuctionBackend.Api.Controllers.ControllerFilter;
using AuctionBackend.Api.RemoteSchema.V1.Item;
using AuctionBackend.Application.Actions.Items;
using AuctionBackend.Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllItems()
        {
            throw new NotImplementedException();
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
