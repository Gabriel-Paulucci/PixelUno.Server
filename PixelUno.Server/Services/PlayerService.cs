using PixelUno.Server.Models;
using PixelUno.Server.Services.Interfaces;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;

namespace PixelUno.Server.Services;

[Service<IPlayerService>]
public class PlayerService : IPlayerService
{
    public PlayerViewModel CreatePlayer(string connectionId, string name)
    {
        var player = new Player(connectionId, name);

        return player;
    }
}