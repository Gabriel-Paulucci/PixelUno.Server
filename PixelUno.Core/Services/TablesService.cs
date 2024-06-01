using System.Collections.Concurrent;
using PixelUno.Core.Services.Interfaces;
using PixelUno.Shared.Exceptions;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.AutoDependencyInjection;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;

namespace PixelUno.Core.Services;

[Service<ITablesService>(LifeTime.Singleton)]
public class TablesService : ITablesService
{
    private readonly ConcurrentDictionary<string, TableViewModel> _tables = [];

    public void AddTable(TableViewModel table)
    {
        if (!_tables.TryAdd(table.Id, table))
            throw new GameException("Fail create table");
    }
    
    public TableViewModel? GetTable(string tableId)
    {
        return _tables.GetValueOrDefault(tableId);
    }
}