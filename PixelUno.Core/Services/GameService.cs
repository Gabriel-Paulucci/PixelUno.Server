using PixelUno.Core.Services.Interfaces;
using PixelUno.Shared.Exceptions;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;
using TakasakiStudio.Lina.Utils.Helpers;

namespace PixelUno.Core.Services;

[Service<IGameService>]
public class GameService(ITablesService tablesService) : IGameService
{
    public TableViewModel CreateTable()
    {
        var table = new TableViewModel
        {
            Id = IdBuilder.Generate()
        };
        tablesService.AddTable(table);
        
        return table;
    }

    public TableViewModel JoinTable(PlayerViewModel player, string tableId)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException("Messa não encontrada");

        if (table.Started)
            throw new GameException("O jogo ja começou");
        
        table.Players.AddLast(player);

        return table;
    }

    public void StartGame(TableViewModel table)
    {
        table.Started = true;
        table.CurrentPlayer = table.Players.First;
    }
}