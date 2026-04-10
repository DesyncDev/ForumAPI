using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Application.Commom.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken ct);
    }
}