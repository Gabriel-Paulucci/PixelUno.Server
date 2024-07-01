using PixelUno.Server.Models;

namespace PixelUno.Server.Services.Interfaces;

public interface ITablesService
{
    void AddTable(Table table);
    Table? GetTable(string tableId);
}