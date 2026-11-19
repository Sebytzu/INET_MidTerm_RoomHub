using System.Collections.Concurrent;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure;

// Temporary in-memory storage (lost on restart). Replaced by EF Core + SQLite at level C.
public sealed class InMemoryBookingStore
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    public void Add(Booking booking) => _bookings[booking.Id] = booking;

    public Booking? Find(Guid id) =>
        _bookings.TryGetValue(id, out var booking) ? booking : null;

    public IReadOnlyList<Booking> ListForRoom(Guid roomId) =>
        _bookings.Values.Where(b => b.RoomId == roomId).ToList();
}
