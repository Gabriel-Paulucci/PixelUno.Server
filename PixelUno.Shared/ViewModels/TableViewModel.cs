using PixelUno.Shared.Enums;

namespace PixelUno.Shared.ViewModels;

public class TableViewModel
{
    public required string Id { get; set; }
    public LinkedList<PlayerViewModel> Players { get; set; } = [];
    public bool Started { get; set; }
    public DeckViewModel Deck { get; } = new();
    public int CardsToBuy { get; set; } = 0;
    public CardViewModel? LastCard { get; set; }
    public LinkedListNode<PlayerViewModel>? CurrentPlayer { get; set; }

    private bool _rightDirection = true;

    public bool CheckCard(CardViewModel card)
    {
        if (CardsToBuy > 0)
        {
            return card.Symbol switch
            {
                CardSymbol.Plus4 => true,
                CardSymbol.Plus2 when LastCard?.Symbol == CardSymbol.Plus2 => true,
                _ => false
            };
        }

        return LastCard is null ||
               card.Color == CardColor.Wild ||
               card.Color == LastCard.Color ||
               card.Symbol == LastCard.Symbol;
    }

    public void AddCard(CardViewModel card)
    {
        switch (card)
        {
            case { Symbol: CardSymbol.Plus2 }:
                CardsToBuy += 2;
                break;
            case { Symbol: CardSymbol.Plus4 }:
                CardsToBuy += 4;
                break;
            case { Symbol: CardSymbol.Reverse }:
                _rightDirection = !_rightDirection;
                break;
            case { Symbol: CardSymbol.Block }:
                NextPlayer();
                break;
        }

        NextPlayer();
        LastCard = card;
    }

    private void NextPlayer()
    {
        CurrentPlayer = _rightDirection
            ? CurrentPlayer?.Next ?? Players.First
            : CurrentPlayer?.Previous ?? Players.Last;
    }
}