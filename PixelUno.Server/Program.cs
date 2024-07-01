using PixelUno.Server.Hubs;
using TakasakiStudio.Lina.AutoDependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddAutoDependencyInjection<Program>();

var app = builder.Build();

app.MapHub<GameHub>("/hubs/game");

await app.RunAsync();