using PixelUno.Shared.ViewModels;

namespace PixelUno.Core.Services.Interfaces;

public interface ITableManagerService
{
    TableViewModel CreateTable(PlayerViewModel player);
}