using PixelUno.Shared.Enums;

namespace PixelUno.Shared.ViewModels;

public class DeckViewModel
{
    private Queue<CardViewModel> Cards { get; set; } = new(Generate());

    private static CardViewModel[] Generate()
    {
        var cards = new List<CardViewModel>();
        cards.AddRange(ColorCards(CardColor.Blue));
        cards.AddRange(ColorCards(CardColor.Yellow));
        cards.AddRange(ColorCards(CardColor.Red));
        cards.AddRange(ColorCards(CardColor.Green));
        cards.AddRange(WildCards());

        var cardsArray = cards.ToArray();
        new Random().Shuffle(cardsArray);
        
        return cardsArray;
    }

    private static List<CardViewModel> ColorCards(CardColor color)
    {
        return
        [
            new CardViewModel(color, CardSymbol.Zero),
            new CardViewModel(color, CardSymbol.One),
            new CardViewModel(color, CardSymbol.One),
            new CardViewModel(color, CardSymbol.Two),
            new CardViewModel(color, CardSymbol.Two),
            new CardViewModel(color, CardSymbol.Three),
            new CardViewModel(color, CardSymbol.Three),
            new CardViewModel(color, CardSymbol.Four),
            new CardViewModel(color, CardSymbol.Four),
            new CardViewModel(color, CardSymbol.Five),
            new CardViewModel(color, CardSymbol.Five),
            new CardViewModel(color, CardSymbol.Six),
            new CardViewModel(color, CardSymbol.Six),
            new CardViewModel(color, CardSymbol.Seven),
            new CardViewModel(color, CardSymbol.Seven),
            new CardViewModel(color, CardSymbol.Eight),
            new CardViewModel(color, CardSymbol.Eight),
            new CardViewModel(color, CardSymbol.Nine),
            new CardViewModel(color, CardSymbol.Nine),
            new CardViewModel(color, CardSymbol.Block),
            new CardViewModel(color, CardSymbol.Block),
            new CardViewModel(color, CardSymbol.Reverse),
            new CardViewModel(color, CardSymbol.Reverse),
            new CardViewModel(color, CardSymbol.Plus2),
            new CardViewModel(color, CardSymbol.Plus2)
        ];
    }

    private static List<CardViewModel> WildCards()
    {
        return 
        [
            new CardViewModel(CardColor.Wild, CardSymbol.Color),
            new CardViewModel(CardColor.Wild, CardSymbol.Color),
            new CardViewModel(CardColor.Wild, CardSymbol.Color),
            new CardViewModel(CardColor.Wild, CardSymbol.Color),
            new CardViewModel(CardColor.Wild, CardSymbol.Plus4),
            new CardViewModel(CardColor.Wild, CardSymbol.Plus4),
            new CardViewModel(CardColor.Wild, CardSymbol.Plus4),
            new CardViewModel(CardColor.Wild, CardSymbol.Plus4)
        ];
    }
    
    public CardViewModel GetNextCard()
    {
        if (Cards.Count == 0)
            Cards = new Queue<CardViewModel>(Generate());
        
        return Cards.Dequeue();
    }
}