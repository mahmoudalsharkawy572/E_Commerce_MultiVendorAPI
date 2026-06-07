using ECommerce.Application.Authentication.Commands.Login;
using ECommerce.Application.Authentication.Commands.RefreshAccessToken;
using ECommerce.Application.Authentication.Commands.Register;
using ECommerce.Application.Authentication.Dtos;
using ECommerce.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<AppUserDto>>> Register([FromBody] RegisterCommand registerCommand)
        {
            var appUserDto = await _mediator.Send(registerCommand);
            var response = ApiResponse<AppUserDto>.CreatedAt(appUserDto, "User Created Successfully!");
            return CreatedAtAction(nameof(Register), response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<TokenDto>>> Login([FromBody] LoginCommand loginCommand)
        {
            var authResponse = await _mediator.Send(loginCommand);
            var response = ApiResponse<TokenDto>.Ok(authResponse, "User Logged in Successfully!");
            return Ok(response);
        }

        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<ApiResponse<TokenDto>>> RefreshAccessToken(RefreshAccessTokenCommand command)
        {
            var tokenDto = await _mediator.Send(command);
            var apiResponse = ApiResponse<TokenDto>.Ok(tokenDto, "Access Token Refreshed Successfully!");
            return Ok(apiResponse);
        }
    }
}
