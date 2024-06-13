namespace PixelUno.Shared.ViewModels;

public class TableViewModel
{
    public required string Id { get; set; }
    public IList<PlayerViewModel> Players { get; set; } = [];
    public bool Started { get; set; }
    public DeckViewModel Deck { get; } = new();
}