using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Methods

        // === Queries (Read-Only) ===
        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken ct)
        => await _context.users
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(ct);

        // === Command (With tracking, for updates) ===
    }
}