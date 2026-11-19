using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Infrastructure.InMemory;

// Temporary (level B): in memory, changes are visible immediately, nothing to save.
public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(0);
}