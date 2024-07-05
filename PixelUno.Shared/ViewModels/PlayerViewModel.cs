using PixelUno.Shared.Enums;

namespace PixelUno.Shared.ViewModels;

public record PlayerViewModel(string Id, string Name, int CardAmount, TableAction Action);
