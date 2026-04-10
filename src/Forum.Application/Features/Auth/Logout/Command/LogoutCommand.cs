using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.OneOf;
using MediatR;
using OneOf;

namespace Forum.Application.Features.Auth.Logout.Command
{
    public record LogoutCommand
    (
        string RefreshToken
    ) : IRequest<OneOf<LogoutResponse, AppError>>;
}