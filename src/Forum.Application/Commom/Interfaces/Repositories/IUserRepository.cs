using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Entities;

namespace Forum.Application.Commom.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // === Queries (Read-Only) ===
        Task<User?> GetUserByEmailAsync(string email, CancellationToken ct);

        // === Commands (With trackig, for updates) ===
    }
}