using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionBackend.API.RemoteSchema.V1;
using OnlineAuctionBackend.API.RemoteSchema.V1.Authentication;
using OnlineAuctionBackend.Identity.Actions.Commands;
using OnlineAuctionBackend.Identity.Actions.Queries;

namespace OnlineAuctionBackend.API.Controllers.V1
{
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public Authentication(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpPost(Manifest.Login)]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = _mapper.Map<LoginCommand>(request);

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                value => Ok(_mapper.Map<LoginResponse>(value)),
                error => BadRequest(_mapper.Map<ErrorResponse>(error)));
        }

        [AllowAnonymous]
        [HttpPost(Manifest.CreateUser)]
        [ProducesResponseType(typeof(CreateUserResponse), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var command = _mapper.Map<CreateUserCommand>(request);

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                value => Ok(_mapper.Map<CreateUserResponse>(value)),
                error => BadRequest(_mapper.Map<ErrorResponse>(error)));
        }

        [Authorize]
        [HttpGet(Manifest.GetUser)]
        [ProducesResponseType(typeof(RemoteUser), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> GetMyUser()
        {
            var userId = HttpContext.GetUserId();
            var result = await _mediator.Send(new GetUserQuery(userId));

            if (result is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RemoteUser>(result));
        }
    }
}
