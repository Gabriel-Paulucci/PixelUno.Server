using System.Collections.Concurrent;
using PixelUno.Server.Models;
using PixelUno.Server.Services.Interfaces;
using PixelUno.Shared.Exceptions;
using TakasakiStudio.Lina.AutoDependencyInjection;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;

namespace PixelUno.Server.Services;

[Service<ITablesService>(LifeTime.Singleton)]
public class TablesService : ITablesService
{
    private readonly ConcurrentDictionary<string, Table> _tables = [];

    public void AddTable(Table table)
    {
        if (!_tables.TryAdd(table.Id, table))
            throw new GameException("Fail create table");
    }
    
    public Table? GetTable(string tableId)
    {
        return _tables.GetValueOrDefault(tableId);
    }
}