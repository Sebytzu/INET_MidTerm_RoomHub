using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class RoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    // Add only marks the entity; IUnitOfWork saves.
    public void Add(Room room) => db.Rooms.Add(room);
}