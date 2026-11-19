using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public sealed record Response(Guid Id, string Name, int Capacity);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetRoomById");

    private static Results<Ok<Response>, ProblemHttpResult> Handle(
        Guid id,
        InMemoryRoomStore store)
    {
        var room = store.Find(id);

        if (room is null)
            return TypedResults.Problem(
                detail: "The room does not exist.",
                statusCode: StatusCodes.Status404NotFound,
                title: "RoomNotFound");

        return TypedResults.Ok(new Response(room.Id, room.Name, room.Capacity));
    }
}
