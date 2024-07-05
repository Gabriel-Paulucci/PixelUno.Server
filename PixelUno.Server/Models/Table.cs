using PixelUno.Shared.Enums;
using PixelUno.Shared.ViewModels;
using TakasakiStudio.Lina.Database.Models;
using TakasakiStudio.Lina.Utils.Helpers;

namespace PixelUno.Server.Models;

public class Table : BaseEntity<string>
{
    public LinkedList<Player> Players { get; set; } = [];
    public bool Started { get; set; }
    public Deck Deck { get; } = new();
    public int CardsToBuy { get; set; } = 0;
    public Card? LastCard { get; set; }
    public LinkedListNode<Player>? CurrentPlayer { get; set; }

    private bool _rightDirection = true;

    public string ChannelName => $"table:{Id}";

    public Table()
    {
        Id = IdBuilder.Generate();
    }

    public bool AddPlayer(PlayerViewModel player)
    {
        var possiblePlayer = Players.FirstOrDefault(x => x.Name == player.Name);

        if (possiblePlayer is not null)
        {
            possiblePlayer.Id = player.Id;
            return true;
        }
        
        Players.AddLast(player);
        return false;
    }

    public bool StartGame()
    {
        if (Started)
            return false;

        Started = true;
        CurrentPlayer = Players.First;
        CurrentPlayer!.Value.Action = TableAction.Playing;
        GetNextPlayer()!.Value.Action = TableAction.Next;
        return true;
    }

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
        CurrentPlayer!.Value.Action = TableAction.Idle;

        switch (card)
        {
            case { Symbol: CardSymbol.Plus2 }:
                CardsToBuy += 2;
                break;
            case { Symbol: CardSymbol.Plus4 }:
                CardsToBuy += 4;
                break;
            case { Symbol: CardSymbol.Reverse }:
                GetNextPlayer()!.Value.Action = TableAction.Idle;
                _rightDirection = !_rightDirection;
                break;
            case { Symbol: CardSymbol.Block }:
                CurrentPlayer = GetNextPlayer();
                CurrentPlayer!.Value.Action = TableAction.Block;
                break;
        }

        CurrentPlayer = GetNextPlayer();
        CurrentPlayer!.Value.Action = TableAction.Playing;
        GetNextPlayer()!.Value.Action = TableAction.Next;
        LastCard = card;
    }

    private LinkedListNode<Player>? GetNextPlayer()
    {
        return _rightDirection
            ? CurrentPlayer?.Next ?? Players.First
            : CurrentPlayer?.Previous ?? Players.Last;
    }

    public IEnumerable<Card> BuyCards()
    {
        if (CardsToBuy <= 0) 
            return [Deck.GetNextCard()];
        
        var cards = Enumerable.Range(0, CardsToBuy).Select(_ => Deck.GetNextCard());
        CardsToBuy = 0;
        CurrentPlayer!.Value.Action = TableAction.Block;
        CurrentPlayer = GetNextPlayer();
        CurrentPlayer!.Value.Action = TableAction.Playing;
        GetNextPlayer()!.Value.Action = TableAction.Next;

        return cards;
    }
    
    public IEnumerable<Card> NextCards(int amount)
    {
        return Enumerable.Range(0, amount).Select(_ => Deck.GetNextCard());
    }

    public Player GetPlayer(string playerId)
    {
        return Players.First(x => x.Id == playerId);
    }

    public bool CheckPlayer(Player player)
    {
        var hasPlayer = Players.Any(x => x.Name == player.Name);

        if (Started && hasPlayer)
            return true;

        return !Started || hasPlayer;
    }

    public static implicit operator TableViewModel(Table table)
    {
        return new TableViewModel(table.Id, table.ChannelName);
    }
}