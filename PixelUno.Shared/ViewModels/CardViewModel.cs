using System.Diagnostics.CodeAnalysis;
using PixelUno.Shared.Enums;

namespace PixelUno.Shared.ViewModels;

public class CardViewModel
{
    public required CardColor Color { get; set; }
    public required CardSymbol Symbol { get; set; }

    [SetsRequiredMembers]
    public CardViewModel(CardColor color, CardSymbol symbol)
    {
        Color = color;
        Symbol = symbol;
    }
}