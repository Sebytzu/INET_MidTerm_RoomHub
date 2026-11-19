using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

// Value object: a valid UTC interval. Half-open: [StartUtc, EndUtc).
public sealed record DateRange
{
    private DateRange()
    {
    }

    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }

    public TimeSpan Duration => EndUtc - StartUtc;

    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        var startUtc = ToUtc(start);
        var endUtc = ToUtc(end);

        // R3: the end must be strictly after the start.
        if (endUtc <= startUtc)
            return DateRangeErrors.EndBeforeStart;

        return new DateRange { StartUtc = startUtc, EndUtc = endUtc };
    }

    // R7: intervals that only touch (one ends when the other starts) do not overlap.
    public bool Overlaps(DateRange other) =>
        StartUtc < other.EndUtc && other.StartUtc < EndUtc;

    public static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}

public static class DateRangeErrors
{
    public static readonly Error EndBeforeStart =
        Error.Validation("InvalidRange", "The end must be after the start.");
}