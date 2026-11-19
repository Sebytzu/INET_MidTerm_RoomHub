namespace RoomHub.Api.Domain.Common;

// "No value" for operations that only succeed or fail (for example cancel).
public readonly record struct Unit
{
    public static readonly Unit Value = new();
}