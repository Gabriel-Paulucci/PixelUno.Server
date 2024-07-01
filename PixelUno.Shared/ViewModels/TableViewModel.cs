using PixelUno.Shared.Enums;

namespace PixelUno.Shared.ViewModels;

public record TableViewModel(string Id)
{
    public string ChannelName => $"table:{Id}";
}
