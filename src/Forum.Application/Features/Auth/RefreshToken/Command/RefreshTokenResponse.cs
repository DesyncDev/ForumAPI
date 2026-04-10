using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Application.Features.Auth.RefreshToken.Command
{
    public record RefreshTokenResponse
    (
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpires
    );
}