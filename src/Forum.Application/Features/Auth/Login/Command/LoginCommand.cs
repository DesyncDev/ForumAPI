using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.OneOf;
using MediatR;
using OneOf;

namespace Forum.Application.Features.Login.Command
{
    public record LoginCommand
    (
        string Email,
        string Password
    ) : IRequest<OneOf<LoginResponse, AppError>>;
}