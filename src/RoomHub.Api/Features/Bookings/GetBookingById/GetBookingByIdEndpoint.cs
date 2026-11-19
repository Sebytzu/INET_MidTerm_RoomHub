using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public static class GetBookingByIdEndpoint
{
    public sealed record Response(
        Guid Id,
        Guid RoomId,
        string BookedBy,
        DateTime StartUtc,
        DateTime EndUtc,
        int Attendees,
        BookingStatus Status);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetBookingById");

    private static Results<Ok<Response>, ProblemHttpResult> Handle(
        Guid id,
        InMemoryBookingStore store)
    {
        var b = store.Find(id);

        if (b is null)
            return TypedResults.Problem(
                detail: "The booking does not exist.",
                statusCode: StatusCodes.Status404NotFound,
                title: "BookingNotFound");

        return TypedResults.Ok(new Response(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status));
    }
}
