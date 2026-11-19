using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public enum BookingStatus
{
    Active,
    Cancelled
}

public sealed class Booking
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);
    public static readonly TimeSpan MaxDurationLargeRoom = TimeSpan.FromHours(2);
    public const int LargeRoomCapacity = 10;
    public const int MaxActiveBookingsPerDay = 2;
    public static readonly TimeSpan MinCancelNotice = TimeSpan.FromMinutes(60);

    private Booking()
    {
    }

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateRange Range { get; private set; } = null!;
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    public bool IsActive => Status == BookingStatus.Active;

    // roomBookings: the existing bookings of the room (for R7).
    // bookedByBookings: the existing bookings of the same person on that day (for R9).
    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> roomBookings,
        IEnumerable<Booking> bookedByBookings)
    {
        // R2
        if (string.IsNullOrWhiteSpace(bookedBy))
            return BookingErrors.BookedByRequired;

        // R3
        var rangeResult = DateRange.Create(startUtc, endUtc);
        if (rangeResult.IsFailure)
            return rangeResult.Error;

        var range = rangeResult.Value;

        // R5: "now" is a parameter that comes from TimeProvider.
        if (range.StartUtc < nowUtc)
            return BookingErrors.StartInPast;

        // R6 + R10: 4 hours, or 2 hours for rooms with capacity greater than 10.
        var maxDuration = room.Capacity > LargeRoomCapacity ? MaxDurationLargeRoom : MaxDuration;
        if (range.Duration > maxDuration)
            return BookingErrors.DurationTooLong(maxDuration);

        // R4
        if (attendees <= 0)
            return BookingErrors.InvalidAttendees;

        if (attendees > room.Capacity)
            return BookingErrors.AttendeesExceedCapacity;

        // R7: only active bookings of the same room conflict.
        if (roomBookings.Any(b => b.IsActive && b.RoomId == room.Id && b.Range.Overlaps(range)))
            return BookingErrors.RoomOccupied;

        // R9: at most 2 active bookings per person per UTC day (day of startUtc), across all rooms.
        var name = bookedBy.Trim();
        var sameDay = bookedByBookings.Count(b =>
            b.IsActive && b.BookedBy == name && b.Range.StartUtc.Date == range.StartUtc.Date);
        if (sameDay >= MaxActiveBookingsPerDay)
            return BookingErrors.DailyLimitReached;

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = name,
            Range = range,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    // R8 + R11
    public Result<Unit> Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
            return BookingErrors.AlreadyCancelled;

        if (Range.StartUtc - nowUtc < MinCancelNotice)
            return BookingErrors.CancelTooLate;

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }
}