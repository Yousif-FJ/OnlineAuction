using AuctionBackend.Api.RemoteSchema.V1;
using AuctionBackend.Api.RemoteSchema.V1.Auction;
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
        [HttpGet(Manifest.GetMyItems)]
        [ProducesResponseType(typeof(ItemRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> GetMyItems()
        {
            var result = await mediator.Send(new GetMyItemsQuery());
            var response = await result
                .ProjectTo<ItemRemote>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(response);
        }

        [HttpPost(Manifest.PostItem)]
        [ProducesResponseType(typeof(ItemRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> PostItem(CreateItemRequest request)
        {
            var item = mapper.Map<Item>(request);
            var command = new AddItemCommand(item);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<ItemRemote>(result));
        }

        [HttpPost(Manifest.PostItemPhoto)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> PostItem([FromForm]AddItemPhotoRequest request)
        {
            var command = new AddItemPictureCommand(request.Id, request.Photo);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete(Manifest.DeleteItem)]
        [ProducesResponseType(typeof(ItemRemote), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var command = new DeleteItemCommand(itemId);
            var result = await mediator.Send(command);
            return Ok(mapper.Map<ItemRemote>(result));
        }
    }
}
