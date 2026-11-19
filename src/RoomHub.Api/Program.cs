using System.Text.Json.Serialization;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Rooms;
using RoomHub.Api.Infrastructure.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Enums appear in JSON as text (for example "Active").
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Errors not produced by the endpoints (for example invalid JSON) are also ProblemDetails.
builder.Services.AddProblemDetails();

// "Now" comes from TimeProvider, never from DateTime.UtcNow.
builder.Services.AddSingleton(TimeProvider.System);

// Level B: in-memory implementations of the domain contracts (EF Core at level C).
builder.Services.AddSingleton<IRoomRepository, InMemoryRoomRepository>();
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();

builder.Services.AddHandlers(typeof(Program).Assembly);

var app = builder.Build();

app.UseStatusCodePages();

app.MapGet("/", () => "RoomHub API is running.");
app.MapRooms();
app.MapBookings();

app.Run();