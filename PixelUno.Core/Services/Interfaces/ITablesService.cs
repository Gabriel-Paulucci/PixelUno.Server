using PixelUno.Shared.ViewModels;

namespace PixelUno.Core.Services.Interfaces;

public interface ITablesService
{
    void AddTable(TableViewModel table);
    TableViewModel? GetTable(string tableId);
}