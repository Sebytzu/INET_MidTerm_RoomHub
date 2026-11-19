namespace RoomHub.Api.Domain.Rooms;

// Level A: still an anemic model. It becomes a rich domain class at level B.
public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
}
