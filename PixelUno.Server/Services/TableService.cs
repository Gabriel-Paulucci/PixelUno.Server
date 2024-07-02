using Microsoft.AspNetCore.SignalR;
using PixelUno.Server.Hubs;
using PixelUno.Server.Hubs.Interfaces;
using PixelUno.Server.Models;
using PixelUno.Server.Services.Interfaces;
using PixelUno.Shared.Constants;
using PixelUno.Shared.Exceptions;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.AutoDependencyInjection;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;

namespace PixelUno.Server.Services;

[Service<ITableService>]
public class TableService(ITablesService tablesService, IHubContext<GameHub, IGameHubClient> gameHub) : ITableService
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

    public async Task StartGame(string tableId)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        if (!table.StartGame())
            throw new GameException(GameExceptionMessages.GameStarted);
        
        await gameHub.Clients.Group(table.ChannelName).Start();
        
        foreach (var playerId in table.Players.Select(x => x.Id))
        {
            foreach (var card in await StartGameCards(table.Id, playerId))
            {
                await gameHub.Clients.Client(playerId).AddCard(card);
            }
        }
        
        var tableCard = GetInitialCard(table.Id);
        await gameHub.Clients.Group(table.ChannelName).PlayCard(tableCard);
    }

    public IEnumerable<PlayerViewModel> GetPlayers(string tableId)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        return table.Players.Select(x => (PlayerViewModel)x);
    }

    public async Task<IEnumerable<CardViewModel>> GetNextCards(string tableId, string playerId)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        if (table.CurrentPlayer?.Value.Id != playerId)
            throw new GameException(GameExceptionMessages.NotYourTurn);

        var cards = table.NextCards(1).ToList();
        table.ResetBuyCards();
        var player = table.GetPlayer(playerId);
        player.AddCards(cards);

        await gameHub.Clients.Group(table.ChannelName).UpdatePlayerInfo(player);

        return cards.Select(x => (CardViewModel)x);
    }

    public async Task<IEnumerable<CardViewModel>> StartGameCards(string tableId, string playerId)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        var cards = table.NextCards(StartGameCard).ToList();
        var player = table.GetPlayer(playerId);
        player.AddCards(cards);

        await gameHub.Clients.Group(table.ChannelName).UpdatePlayerInfo(player);

        return cards.Select(x => (CardViewModel)x);
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

    public bool CheckCard(string tableId, string playerId, CardViewModel card)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        if (!table.Started)
            throw new GameException(GameExceptionMessages.GameNotStarted);

        if (table.CurrentPlayer?.Value.Id != playerId)
            throw new GameException(GameExceptionMessages.NotYourTurn);
        
        var player = table.GetPlayer(playerId);
        if (!player.HasCard(card))
            throw new GameException(GameExceptionMessages.NotHaveACard);
        
        return table.CheckCard(card);
    }

    public async Task PlayCard(string tableId, string playerId, CardViewModel card)
    {
        var table = tablesService.GetTable(tableId);

        if (table is null)
            throw new GameException(GameExceptionMessages.TableNotFound);

        if (!table.Started)
            throw new GameException(GameExceptionMessages.GameNotStarted);

        if (table.CurrentPlayer?.Value.Id != playerId)
            throw new GameException(GameExceptionMessages.NotYourTurn);

        var player = table.GetPlayer(playerId);

        if (!player.HasCard(card))
            throw new GameException(GameExceptionMessages.NotHaveACard);

        player.RemoveCard(card);
        var actions = table.AddCard(card);
        
        await gameHub.Clients.Group(table.ChannelName).PlayCard(card);
        await gameHub.Clients.Group(table.ChannelName).UpdatePlayerInfo(player);
        await gameHub.Clients.Group(table.ChannelName)
            .TableNextSteps(actions.Select(x => new TableActionViewModel(x.Item1, x.Item2)));

        if (player.Cards.Count == 0)
            await gameHub.Clients.Group(table.ChannelName).EndGame(player);
    }
}