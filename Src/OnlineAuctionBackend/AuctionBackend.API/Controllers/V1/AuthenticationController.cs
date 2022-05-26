using AuctionBackend.Api.RemoteSchema;
using AuctionBackend.Api.RemoteSchema.V1.Authentication;
using AuctionBackend.Application.Helper;
using AuctionBackend.Identity.Actions.Commands;
using AuctionBackend.Identity.Actions.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuctionBackend.Api.Controllers.V1
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator, IMapper mapper)
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
            Debug.Assert(userId is not null, "User can not be null with correct authorization");
            var result = await _mediator.Send(new GetUserQuery(userId));

            if (result is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RemoteUser>(result));
        }
    }
}
