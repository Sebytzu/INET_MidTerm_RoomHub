using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error NotFound =
        Error.NotFound("BookingNotFound", "The booking does not exist.");

    public static readonly Error BookedByRequired =
        Error.Validation("InvalidBookedBy", "BookedBy is required.");

    public static readonly Error StartInPast =
        Error.Validation("StartInPast", "A booking cannot start in the past.");

    public static Error DurationTooLong(TimeSpan max) =>
        Error.Validation("DurationTooLong", $"A booking for this room can last at most {max.TotalHours:0} hours.");

    public static readonly Error InvalidAttendees =
        Error.Validation("InvalidAttendees", "Attendees must be positive.");

    public static readonly Error AttendeesExceedCapacity =
        Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats.");

    public static readonly Error RoomOccupied =
        Error.Conflict("RoomOccupied", "The room is occupied in this interval.");

    public static readonly Error DailyLimitReached =
        Error.Conflict("DailyLimitReached", "A participant cannot have more than 2 active bookings per day.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("AlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error CancelTooLate =
        Error.Conflict("CancelTooLate", "A booking can be cancelled only at least 60 minutes before it starts.");
}