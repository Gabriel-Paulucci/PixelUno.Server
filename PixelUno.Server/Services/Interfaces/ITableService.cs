using PixelUno.Shared.ViewModels;

namespace PixelUno.Server.Services.Interfaces;

public interface ITableService
{
    TableViewModel CreateTable();
    TableViewModel JoinTable(PlayerViewModel player, string tableId);
    void StartGame(string tableId);
    IEnumerable<PlayerViewModel> GetPlayers(string tableId);
    Task<IEnumerable<CardViewModel>> GetNextCards(string tableId, string playerId);
    CardViewModel GetInitialCard(string tableId);
    Task<IEnumerable<CardViewModel>> StartGameCards(string tableId, string playerId);
    bool CheckCard(string tableId, string playerId, CardViewModel card);
    void PlayCard(string tableId, string playerId, CardViewModel card);
}