using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Application.Features.Login.Command
{
    public record LoginResponse
    (
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpires
    );
}