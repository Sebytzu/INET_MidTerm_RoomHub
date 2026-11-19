using System.Collections.Concurrent;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.InMemory;

// Temporary (level B). Replaced by the EF Core repository at level C.
public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(_bookings.TryGetValue(id, out var booking) ? booking : null);

    public Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<Booking>>(
            _bookings.Values.Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active).ToList());

    public Task<IReadOnlyList<Booking>> ListActiveForBookedByOnDayAsync(
        string bookedBy, DateTime dayUtc, CancellationToken cancellationToken)
    {
        var from = dayUtc.Date;
        var to = from.AddDays(1);
        return Task.FromResult<IReadOnlyList<Booking>>(
            _bookings.Values.Where(b =>
                b.BookedBy == bookedBy &&
                b.Status == BookingStatus.Active &&
                b.Range.StartUtc >= from && b.Range.StartUtc < to).ToList());
    }

    public void Add(Booking booking) => _bookings[booking.Id] = booking;
}