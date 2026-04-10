using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Features.Login.Command;
using Forum.Domain.Commom.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        => _mediator = mediator;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
        {
            var response = await _mediator.Send(command, ct);

            return response.Match(
                success => Ok(success),
                error => error.type switch
                {
                    ErrorTypes.Unauthorized => Unauthorized(error),
                    _ => StatusCode(500, error)
                }
            );
        }
    }
}