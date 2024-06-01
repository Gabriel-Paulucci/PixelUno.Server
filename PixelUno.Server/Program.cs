using PixelUno.Core.Adapters;
using PixelUno.Core.Services.Interfaces;
using PixelUno.Server.Hubs;
using PixelUno.Shared.Config;
using TakasakiStudio.Lina.AutoDependencyInjection;
using TakasakiStudio.Lina.Utils.LoaderConfig;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Services.AddLoaderConfig<IAppConfig>();

builder.Services.AddSingleton(await GarnetAdapter.CreateClient(config));

builder.Services.AddSignalR();
builder.Services.AddAutoDependencyInjection<ITablesService>();

var app = builder.Build();

app.MapHub<GameHub>("/hubs/game");

app.Run();