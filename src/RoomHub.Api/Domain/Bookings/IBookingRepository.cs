namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // R7: active bookings of a room.
    Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken);

    // R9: active bookings of a person that start on the given UTC day.
    Task<IReadOnlyList<Booking>> ListActiveForBookedByOnDayAsync(
        string bookedBy, DateTime dayUtc, CancellationToken cancellationToken);

    void Add(Booking booking);
}