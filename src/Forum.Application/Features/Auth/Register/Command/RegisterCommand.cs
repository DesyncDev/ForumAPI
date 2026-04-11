using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.OneOf;
using MediatR;
using OneOf;

namespace Forum.Application.Features.Register.Command
{
    public sealed record RegisterCommand
    (
        string Username,
        string DisplayName,
        string Email,
        string Password
    ) : IRequest<OneOf<RegisterResponse, AppError>>;
}