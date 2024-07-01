using PixelUno.Shared.Enums;
using TakasakiStudio.Lina.Database.Models;
using TakasakiStudio.Lina.Utils.Helpers;

namespace PixelUno.Server.Models;

public class Deck : BaseEntity<string>
{
    
    private Queue<Card> Cards { get; set; } = new(Generate());
    
    public Deck()
    {
        Id = IdBuilder.Generate();
    }
    
    private static Card[] Generate()
    {
        var cards = new List<Card>();
        cards.AddRange(ColorCards(CardColor.Blue));
        cards.AddRange(ColorCards(CardColor.Yellow));
        cards.AddRange(ColorCards(CardColor.Red));
        cards.AddRange(ColorCards(CardColor.Green));
        cards.AddRange(WildCards());

        var cardsArray = cards.ToArray();
        new Random().Shuffle(cardsArray);
        
        return cardsArray;
    }

    private static List<Card> ColorCards(CardColor color)
    {
        return
        [
            new Card(color, CardSymbol.Zero),
            new Card(color, CardSymbol.One),
            new Card(color, CardSymbol.One),
            new Card(color, CardSymbol.Two),
            new Card(color, CardSymbol.Two),
            new Card(color, CardSymbol.Three),
            new Card(color, CardSymbol.Three),
            new Card(color, CardSymbol.Four),
            new Card(color, CardSymbol.Four),
            new Card(color, CardSymbol.Five),
            new Card(color, CardSymbol.Five),
            new Card(color, CardSymbol.Six),
            new Card(color, CardSymbol.Six),
            new Card(color, CardSymbol.Seven),
            new Card(color, CardSymbol.Seven),
            new Card(color, CardSymbol.Eight),
            new Card(color, CardSymbol.Eight),
            new Card(color, CardSymbol.Nine),
            new Card(color, CardSymbol.Nine),
            new Card(color, CardSymbol.Block),
            new Card(color, CardSymbol.Block),
            new Card(color, CardSymbol.Reverse),
            new Card(color, CardSymbol.Reverse),
            new Card(color, CardSymbol.Plus2),
            new Card(color, CardSymbol.Plus2)
        ];
    }

    private static List<Card> WildCards()
    {
        return 
        [
            new Card(CardColor.Wild, CardSymbol.Color),
            new Card(CardColor.Wild, CardSymbol.Color),
            new Card(CardColor.Wild, CardSymbol.Color),
            new Card(CardColor.Wild, CardSymbol.Color),
            new Card(CardColor.Wild, CardSymbol.Plus4),
            new Card(CardColor.Wild, CardSymbol.Plus4),
            new Card(CardColor.Wild, CardSymbol.Plus4),
            new Card(CardColor.Wild, CardSymbol.Plus4)
        ];
    }
    
    public Card GetNextCard()
    {
        if (Cards.Count == 0)
            Cards = new Queue<Card>(Generate());
        
        return Cards.Dequeue();
    }
}