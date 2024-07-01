using PixelUno.Shared.ViewModels;

namespace PixelUno.Server.Services.Interfaces;

public interface IPlayerService
{
    PlayerViewModel CreatePlayer(string connectionId, string name);
}