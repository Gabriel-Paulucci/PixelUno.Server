using PixelUno.Core.Services.Interfaces;
using PixelUno.Server.Hubs;
using TakasakiStudio.Lina.AutoDependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddAutoDependencyInjection<ITablesService>();

var app = builder.Build();

app.MapHub<GameHub>("/hubs/game");

app.Run();