using PixelUno.Core.Adapters.Interfaces;
using PixelUno.Shared.Config;
using StackExchange.Redis;
using TakasakiStudio.Lina.AutoDependencyInjection.Attributes;

namespace PixelUno.Core.Adapters;

[Adapter<IGarnetAdapter>]
public class GarnetAdapter(ConnectionMultiplexer connection) : IGarnetAdapter
{
    public static async Task<ConnectionMultiplexer> CreateClient(IAppConfig appConfig)
    {
        var client = await ConnectionMultiplexer.ConnectAsync(new ConfigurationOptions()
        {
            EndPoints =
            {
                appConfig.Garnet.Url
            }
        });

        return client;
    }
}