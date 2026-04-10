using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Application.Commom.Interfaces.Repositories;
using Forum.Infrastructure.Data;

namespace Forum.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
    }
}