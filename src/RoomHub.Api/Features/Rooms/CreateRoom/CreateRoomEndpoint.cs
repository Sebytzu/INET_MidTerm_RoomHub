using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public static class CreateRoomEndpoint
{
    public sealed record Request(string? Name, int Capacity);

    public sealed record Response(Guid Id, string Name, int Capacity);

    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/", Handle)
            .WithName("CreateRoom");

    // Level A: validation is temporarily in the endpoint. It moves into Room.Create at level B.
    private static Results<Created<Response>, ProblemHttpResult> Handle(
        Request request,
        InMemoryRoomStore store)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return TypedResults.Problem(
                detail: "Room name is required.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "InvalidName");

        if (request.Capacity <= 0)
            return TypedResults.Problem(
                detail: "Capacity must be positive.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "InvalidCapacity");

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Capacity = request.Capacity
        };

        store.Add(room);

        return TypedResults.Created(
            $"/api/rooms/{room.Id}",
            new Response(room.Id, room.Name, room.Capacity));
    }
}
