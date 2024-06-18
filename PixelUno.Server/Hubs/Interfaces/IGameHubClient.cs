using PixelUno.Shared.ViewModels;

namespace PixelUno.Server.Hubs.Interfaces;

public interface IGameHubClient
{
    Task JoinPlayer(PlayerViewModel player);
    Task Start();
    Task AddCard(CardViewModel card);
    Task PlayCard(CardViewModel card);
}