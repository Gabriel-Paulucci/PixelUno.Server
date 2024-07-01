using PixelUno.Shared.ViewModels;

namespace PixelUno.Server.Services.Interfaces;

public interface ITableService
{
    TableViewModel CreateTable();
    TableViewModel JoinTable(PlayerViewModel player, string tableId);
    void StartGame(string tableId);
    IEnumerable<PlayerViewModel> GetPlayers(string tableId);
    IEnumerable<CardViewModel> GetNextCards(string tableId, PlayerViewModel player);
    CardViewModel GetInitialCard(string tableId);
    IEnumerable<CardViewModel> StartGameCards(string tableId);
    bool CheckCard(string tableId, PlayerViewModel player, CardViewModel card);
    void PlayCard(string tableId, PlayerViewModel player, CardViewModel card);
}