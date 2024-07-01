using PixelUno.Server.Models;
using PixelUno.Server.Services.Interfaces;
using PixelUno.Shared.Constants;
using PixelUno.Shared.Exceptions;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;

namespace PixelUno.Server.Services;

[Service<ITableService>]
public class TableService(ITablesService tablesService) : ITableService
{
    private const int StartGameCard = 7;
    
    public TableViewModel CreateTable()
    {
        var table = new Table();
        
        tablesService.AddTable(table);
        
        return table;
    }

    public TableViewModel JoinTable(PlayerViewModel player, string tableId)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        if (table.Started)
            throw new GameException(GameExceptionMessages.GameStarted);
        
        if (!table.AddPlayer(player))
            throw new GameException(GameExceptionMessages.FullGame);

        return table;
    }

    public void StartGame(string tableId)
    {
        var table = tablesService.GetTable(tableId);
        
        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);
        
        if (!table.StartGame())
            throw new GameException(GameExceptionMessages.GameStarted);
    }

    public IEnumerable<PlayerViewModel> GetPlayers(string tableId)
    {
        var table = tablesService.GetTable(tableId);
        
        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        return table.Players.Select(x => (PlayerViewModel)x);
    }

    public IEnumerable<CardViewModel> GetNextCards(string tableId, PlayerViewModel player)
    {
        var table = tablesService.GetTable(tableId);
        
        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);
        
        if (table.CurrentPlayer?.Value.Id != player.Id)
            throw new GameException(GameExceptionMessages.NotYourTurn);

        return table.NextCards(1).Select(x => (CardViewModel)x);
    }
    
    public IEnumerable<CardViewModel> StartGameCards(string tableId)
    {
        var table = tablesService.GetTable(tableId);
        
        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        return table.NextCards(StartGameCard).Select(x => (CardViewModel)x);
    }
    
    public CardViewModel GetInitialCard(string tableId)
    {
        var table = tablesService.GetTable(tableId);
        
        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        var card = table.NextCards(1).First();
        card.RemoveWild();
        table.LastCard = card;
        return card;
    }

    public bool CheckCard(string tableId, PlayerViewModel player, CardViewModel card)
    {
        var table = tablesService.GetTable(tableId);
        
        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);
        
        if (!table.Started)
            throw new GameException(GameExceptionMessages.GameNotStarted);

        if (table.CurrentPlayer?.Value.Id != player.Id)
            throw new GameException(GameExceptionMessages.NotYourTurn);

        return table.CheckCard(card);
    }

    public void PlayCard(string tableId, PlayerViewModel player, CardViewModel card)
    {
        var table = tablesService.GetTable(tableId);
        
        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);
        
        if (!table.Started)
            throw new GameException(GameExceptionMessages.GameNotStarted);

        if (table.CurrentPlayer?.Value.Id != player.Id)
            throw new GameException(GameExceptionMessages.NotYourTurn);
        
        table.AddCard(card);
    }
}