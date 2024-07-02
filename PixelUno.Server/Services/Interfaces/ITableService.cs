using PixelUno.Shared.ViewModels;

namespace PixelUno.Server.Services.Interfaces;

public interface ITableService
{
    TableViewModel CreateTable();
    TableViewModel JoinTable(PlayerViewModel player, string tableId);
    Task StartGame(string tableId);
    IEnumerable<PlayerViewModel> GetPlayers(string tableId);
    Task<IEnumerable<CardViewModel>> GetNextCards(string tableId, string playerId);
    CardViewModel GetInitialCard(string tableId);
    Task<IEnumerable<CardViewModel>> StartGameCards(string tableId, string playerId);
    bool CheckCard(string tableId, string playerId, CardViewModel card);
    Task PlayCard(string tableId, string playerId, CardViewModel card);
}