using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Application.Features.Auth.Logout.Command
{
    public record LogoutResponse
    (
        DateTime LogoutAt
    );
}