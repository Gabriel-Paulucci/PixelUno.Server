using Microsoft.AspNetCore.SignalR;
using PixelUno.Core.Services.Interfaces;
using PixelUno.Shared.Exceptions;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.Utils.Helpers;

namespace PixelUno.Server.Hubs;

public class GameHub(ILogger<GameHub> logger, ITableManagerService tableManagerService) : Hub
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

    public void SetPlayerName(string name)
    {
        Context.Items.Add("player", new PlayerViewModel(IdBuilder.Generate(), name));
    }
    
    public string CreateTable()
    {
        if (!Context.Items.TryGetValue("player", out var value) || value is not PlayerViewModel player)
            throw new GameException("Player not found");

        var table = tableManagerService.CreateTable(player);
        
        Context.Items.Add("table", table);

        return table.Id;
    }
}