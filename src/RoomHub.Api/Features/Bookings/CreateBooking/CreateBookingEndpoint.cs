using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public sealed record Request(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

    public sealed record Response(Guid Id, Guid RoomId, BookingStatus Status);

    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/{roomId:guid}/bookings", Handle)
            .WithName("CreateBooking");

    // Level A: validation is temporarily in the endpoint. It moves into Booking.Create at level B.
    private static Results<Created<Response>, ProblemHttpResult> Handle(
        Guid roomId,
        Request request,
        InMemoryRoomStore rooms,
        InMemoryBookingStore bookings)
    {
        // 404 for a missing room comes before any validation.
        var room = rooms.Find(roomId);
        if (room is null)
            return TypedResults.Problem(
                detail: "The room does not exist.",
                statusCode: StatusCodes.Status404NotFound,
                title: "RoomNotFound");

        if (string.IsNullOrWhiteSpace(request.BookedBy))
            return Validation("InvalidBookedBy", "BookedBy is required.");

        if (request.EndUtc <= request.StartUtc)
            return Validation("InvalidRange", "The end must be after the start.");

        if (request.Attendees <= 0)
            return Validation("InvalidAttendees", "Attendees must be positive.");

        if (request.Attendees > room.Capacity)
            return Validation("AttendeesExceedCapacity", "The room does not have enough seats.");

        foreach (var existing in bookings.ListForRoom(roomId))
        {
            if (existing.StartUtc <= request.EndUtc && request.StartUtc <= existing.EndUtc)
                return TypedResults.Problem(
                    detail: "The room is occupied in this interval.",
                    statusCode: StatusCodes.Status409Conflict,
                    title: "RoomOccupied");
        }

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomId,
            BookedBy = request.BookedBy.Trim(),
            StartUtc = request.StartUtc,
            EndUtc = request.EndUtc,
            Attendees = request.Attendees,
            Status = BookingStatus.Active
        };

        bookings.Add(booking);

        return TypedResults.Created(
            $"/api/bookings/{booking.Id}",
            new Response(booking.Id, booking.RoomId, booking.Status));
    }

    private static ProblemHttpResult Validation(string code, string message) =>
        TypedResults.Problem(
            detail: message,
            statusCode: StatusCodes.Status400BadRequest,
            title: code);
}
