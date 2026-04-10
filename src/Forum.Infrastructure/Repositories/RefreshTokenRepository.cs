using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Domain.Commom.Entities;
using Forum.Infrastructure.Data;

namespace Forum.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        // === Queries (Read-Only) ===
        public void Add(RefreshToken refreshToken)
        => _context.refreshTokens.Add(refreshToken);
    }
}