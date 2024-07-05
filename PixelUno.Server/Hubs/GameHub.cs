using Microsoft.AspNetCore.SignalR;
using PixelUno.Server.Extensions;
using PixelUno.Server.Hubs.Interfaces;
using PixelUno.Server.Services.Interfaces;
using PixelUno.Shared.Enums;
using PixelUno.Shared.ViewModels;

namespace PixelUno.Server.Hubs;

public class GameHub(ILogger<GameHub> logger, ITableService tableService, IPlayerService playerService)
    : Hub<IGameHubClient>
{
    public override Task OnConnectedAsync()
    {
        logger.LogInformation("Client connect: {Id}", Context.ConnectionId);
        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Client disconnect: {Id}", Context.ConnectionId);

        if (exception is not null)
            logger.LogError(exception, "An error occurred");

        return Task.CompletedTask;
    }

    public PlayerViewModel SetPlayerName(string name)
    {
        var player = playerService.CreatePlayer(Context.ConnectionId, name);

        Context.Items.Add(GameContextItems.Player, player);

        return player;
    }

    public string CreateTable()
    {
        var table = tableService.CreateTable();
        return table.Id;
    }

    public async Task JoinTable(string tableId)
    {
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        var (table, alreadyExists) = await tableService.JoinTable(player, tableId);
        Context.Items.Remove(GameContextItems.Table);
        Context.Items.Add(GameContextItems.Table, table);

        await Groups.AddToGroupAsync(Context.ConnectionId, table.ChannelName);

        if (!alreadyExists)
            await Clients.GroupExcept(table.ChannelName, Context.ConnectionId).JoinPlayer(player);
    }

    public async Task StartGame()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);

        await tableService.StartGame(table.Id);
    }

    public async Task BuyCard()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        foreach (var card in await tableService.GetNextCards(table.Id, player.Id))
        {
            await Clients.Client(Context.ConnectionId).AddCard(card);
        }
    }

    public bool CheckCard(CardViewModel card)
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        return tableService.CheckCard(table.Id, player.Id, card);
    }

    public async Task PlayingCard(CardViewModel card)
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        await tableService.PlayCard(table.Id, player.Id, card);
    }

    public IEnumerable<PlayerViewModel> GetPlayers()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);

        return tableService.GetPlayers(table.Id);
    }

    public async Task Leave()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);

        await tableService.Destroy(table.Id);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, table.ChannelName);
    }

    public IEnumerable<CardViewModel> GetMyCards()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        return tableService.GetMyCards(table.Id, player.Id);
    }

    public bool AlreadyStarted()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);

        return tableService.AlreadyStarted(table.Id);
    }

    public CardViewModel? GetTableCard()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);

        return tableService.GetTableCard(table.Id);
    }
}