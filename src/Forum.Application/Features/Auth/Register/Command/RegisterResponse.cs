using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Application.Features.Register.Command
{
    public sealed record RegisterResponse
    (
        Guid Id,
        DateTime CreatedAt
    );
}