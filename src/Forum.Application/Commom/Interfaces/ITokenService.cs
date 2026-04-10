using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Domain.Commom.Entities;
using Forum.Domain.Entities;

namespace Forum.Application.Commom.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime expires) GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(Guid userId);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}