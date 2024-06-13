using Microsoft.AspNetCore.SignalR;
using PixelUno.Core.Services.Interfaces;
using PixelUno.Server.Extensions;
using PixelUno.Server.Hubs.Interfaces;
using PixelUno.Shared.ViewModels;

namespace PixelUno.Server.Hubs;

public class GameHub(ILogger<GameHub> logger, IGameService gameService) : Hub<IGameHubClient>
{
    private const int StartGameCard = 7;
    
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
        var player = new PlayerViewModel
        {
            Id = Context.ConnectionId,
            Name = name
        };
        Context.Items.Add("Player", player);
        return player;
    }

    public string CreateTable()
    {
        var table = gameService.CreateTable();
        return table.Id;
    }

    public async Task<IEnumerable<PlayerViewModel>> JoinTable(string tableId)
    {
        var player = Context.Items.GetValue<PlayerViewModel>("Player");

        var table = gameService.JoinTable(player, tableId);
        Context.Items.Add("Table", table);

        var group = $"table:{table.Id}";

        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await Clients.GroupExcept(group, Context.ConnectionId).JoinPlayer(player);

        return table.Players;
    }

    public async Task StartGame()
    {
        var table = Context.Items.GetValue<TableViewModel>("Table");
        gameService.StartGame(table);
        await Clients.Group($"table:{table.Id}").Start();

        foreach (var player in table.Players)
        {
            for (var i = 0; i < StartGameCard; i++)
            {
                await Clients.Client(player.Id).AddCard(table.Deck.GetNextCard());
            }
        }
    }

    public async Task BuyCard()
    {
        var table = Context.Items.GetValue<TableViewModel>("Table");

        if (table.CardsToBuy > 0)
        {
            for (var i = 0; i < table.CardsToBuy; i++)
            {
                await Clients.Client(Context.ConnectionId).AddCard(table.Deck.GetNextCard());
            }
            return;
        }

        await Clients.Client(Context.ConnectionId).AddCard(table.Deck.GetNextCard());
    }
}