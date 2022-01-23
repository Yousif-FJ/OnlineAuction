using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionBackend.API.Controllers.V1;
using OnlineAuctionBackend.API.RemoteSchema.V1;
using OnlineAuctionBackend.API.RequestResponseSchema.V1.Authentication;

namespace OnlineAuctionBackend.API.Controllers
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
        [HttpPost(ManifestV1.Login)]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var appRequest = _mapper.Map<LoginCommand>(request);

            var actionResult = await _mediator.Send(appRequest);

            if (actionResult.AccessToken is null)
            {
                return BadRequest(_mapper.Map<ErrorResponse>(actionResult));
            }

            return Ok(_mapper.Map<LoginResponse>(actionResult));
        }
    }
}
