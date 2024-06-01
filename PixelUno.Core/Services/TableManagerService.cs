using PixelUno.Core.Services.Interfaces;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;
using TakasakiStudio.Lina.Utils.Helpers;

namespace PixelUno.Core.Services;

[Service<ITableManagerService>]
public class TableManagerService(ITablesService tablesService) : ITableManagerService
{
    public TableViewModel CreateTable(PlayerViewModel player)
    {
        var table = new TableViewModel(IdBuilder.Generate());
        table.Players.Add(player);
        
        tablesService.AddTable(table);

        return table;
    }
}