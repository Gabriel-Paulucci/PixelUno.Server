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

        var table = tableService.JoinTable(player, tableId);
        Context.Items.Add(GameContextItems.Table, table);

        await Groups.AddToGroupAsync(Context.ConnectionId, table.ChannelName);
        await Clients.GroupExcept(table.ChannelName, Context.ConnectionId).JoinPlayer(player);
    }

    public async Task StartGame()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);

        tableService.StartGame(table.Id);
        await Clients.Group(table.ChannelName).Start();

        foreach (var player in tableService.GetPlayers(table.Id))
        {
            foreach (var card in tableService.StartGameCards(table.Id))
            {
                await Clients.Client(player.Id).AddCard(card);
            }
        }

        var tableCard = tableService.GetInitialCard(table.Id);
        await Clients.Group(table.ChannelName).PlayCard(tableCard);
    }

    public async Task BuyCard()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        foreach (var card in tableService.GetNextCards(table.Id, player))
        {
            await Clients.Client(Context.ConnectionId).AddCard(card);
        }
    }

    public bool CheckCard(CardViewModel card)
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        return tableService.CheckCard(table.Id, player, card);
    }

    public async Task PlayingCard(CardViewModel card)
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);
        var player = Context.Items.GetValue<PlayerViewModel>(GameContextItems.Player);

        tableService.PlayCard(table.Id, player, card);
        await Clients.Group(table.ChannelName).PlayCard(card);
    }

    public IEnumerable<PlayerViewModel> GetPlayers()
    {
        var table = Context.Items.GetValue<TableViewModel>(GameContextItems.Table);

        return tableService.GetPlayers(table.Id);
    }
}