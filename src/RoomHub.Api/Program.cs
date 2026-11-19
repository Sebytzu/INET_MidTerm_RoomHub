using System.Text.Json.Serialization;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Rooms;
using RoomHub.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Enums appear in JSON as text (for example "Active").
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Errors not produced by the endpoints (for example invalid JSON) are also ProblemDetails.
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<InMemoryRoomStore>();
builder.Services.AddSingleton<InMemoryBookingStore>();

var app = builder.Build();

app.UseStatusCodePages();

app.MapGet("/", () => "RoomHub API is running.");
app.MapRooms();
app.MapBookings();

app.Run();
