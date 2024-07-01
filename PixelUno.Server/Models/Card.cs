using System.Diagnostics.CodeAnalysis;
using PixelUno.Shared.Enums;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.Database.Models;
using TakasakiStudio.Lina.Utils.Helpers;

namespace PixelUno.Server.Models;

public class Card : BaseEntity<string>
{
    public required CardColor Color { get; set; }
    public required CardSymbol Symbol { get; set; }

    [SetsRequiredMembers]
    public Card(CardColor color, CardSymbol symbol)
    {
        Id = IdBuilder.Generate();
        Color = color;
        Symbol = symbol;
    }

    public void RemoveWild()
    {
        if (Color == CardColor.Wild)
            Color = new List<CardColor>([CardColor.Blue, CardColor.Yellow, CardColor.Red, CardColor.Green])
                [new Random().Next(0, 4)];
    }

    public static implicit operator CardViewModel(Card card)
    {
        return new CardViewModel(card.Color, card.Symbol);
    }

    public static implicit operator Card(CardViewModel model)
    {
        return new Card(model.Color, model.Symbol);
    }
}