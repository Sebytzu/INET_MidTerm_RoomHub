using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(
        Guid roomId, CancellationToken cancellationToken) =>
        await db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Booking>> ListActiveForBookedByOnDayAsync(
        string bookedBy, DateTime dayUtc, CancellationToken cancellationToken)
    {
        var from = DateTime.SpecifyKind(dayUtc.Date, DateTimeKind.Utc);
        var to = from.AddDays(1);

        return await db.Bookings
            .Where(b => b.BookedBy == bookedBy
                        && b.Status == BookingStatus.Active
                        && b.Range.StartUtc >= from
                        && b.Range.StartUtc < to)
            .ToListAsync(cancellationToken);
    }

    // Add only marks the entity; IUnitOfWork saves.
    public void Add(Booking booking) => db.Bookings.Add(booking);
}