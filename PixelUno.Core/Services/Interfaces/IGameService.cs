using PixelUno.Shared.ViewModels;

namespace PixelUno.Core.Services.Interfaces;

public interface IGameService
{
    TableViewModel CreateTable();
    TableViewModel JoinTable(PlayerViewModel player, string tableId);
    void StartGame(TableViewModel table);
}