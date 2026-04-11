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
        // === Others ===
        public void Add(User user)
        => _context.users.Add(user);

        // === Queries (Read-Only) ===
        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken ct)
        => await _context.users
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(ct);

        public async Task<bool> CheckUserByUsernameAsync(string username, CancellationToken ct)
        => await _context.users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username, ct);

        public async Task<bool> CheckUserByEmailAsync(string email, CancellationToken ct)
        => await _context.users
            .AsNoTracking()
            .AnyAsync(e => e.Email == email, ct);

        // === Command (With tracking, for updates) ===
    }
}