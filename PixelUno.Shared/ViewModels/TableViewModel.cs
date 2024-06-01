namespace PixelUno.Shared.ViewModels;

public record TableViewModel(string Id)
{
    public IList<PlayerViewModel> Players { get; set; } = [];
}