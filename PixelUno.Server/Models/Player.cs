using System.Diagnostics.CodeAnalysis;
using PixelUno.Shared.Enums;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.Database.Models;

namespace PixelUno.Server.Models;

public class Player : BaseEntity<string>
{
    public required string Name { get; set; }
    public List<Card> Cards { get; set; } = [];
    public TableAction Action { get; set; } = TableAction.Idle;
    
    [SetsRequiredMembers]
    public Player(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AddCards(List<Card> cards)
    {
        Cards.AddRange(cards);
    }

    public bool HasCard(Card card)
    {
        return Cards.Exists(x => x.Id == card.Id);
    }

    public void RemoveCard(Card card)
    {
        Cards.RemoveAll(x => x.Id == card.Id);
    }

    public static implicit operator PlayerViewModel(Player player)
    {
        return new PlayerViewModel(player.Id, player.Name, player.Cards.Count, player.Action);
    }

    public static implicit operator Player(PlayerViewModel model)
    {
        return new Player(model.Id, model.Name)
        {
            Action = model.Action
        };
    }
}