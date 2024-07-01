using System.Diagnostics.CodeAnalysis;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.Database.Models;

namespace PixelUno.Server.Models;

public class Player : BaseEntity<string>
{
    public required string Name { get; set; }
    
    [SetsRequiredMembers]
    public Player(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public static implicit operator PlayerViewModel(Player player)
    {
        return new PlayerViewModel(player.Id, player.Name);
    }

    public static implicit operator Player(PlayerViewModel model)
    {
        return new Player(model.Id, model.Name);
    }
}