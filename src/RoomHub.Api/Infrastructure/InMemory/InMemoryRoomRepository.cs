using System.Collections.Concurrent;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.InMemory;

// Temporary (level B). Replaced by the EF Core repository at level C.
public sealed class InMemoryRoomRepository : IRoomRepository
{
    private readonly ConcurrentDictionary<Guid, Room> _rooms = new();

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(_rooms.TryGetValue(id, out var room) ? room : null);

    public void Add(Room room) => _rooms[room.Id] = room;
}