using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.OneOf;
using MediatR;
using OneOf;

namespace Forum.Application.Features.Auth.RefreshToken.Command
{
    public record RefreshTokenCommand
    (
        string RefreshToken
    ) : IRequest<OneOf<RefreshTokenResponse, AppError>>;
}