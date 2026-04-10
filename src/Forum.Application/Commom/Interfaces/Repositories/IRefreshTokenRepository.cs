using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Commom.Entities;

namespace Forum.Application.Commom.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        // === Queries (Read-Only) ===
        void Add(RefreshToken refreshToken);

        // === Commands (With trackig, for updates) ===
    }
}