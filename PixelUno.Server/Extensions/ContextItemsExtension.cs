using PixelUno.Shared.Exceptions;

namespace PixelUno.Server.Extensions;

public static class ContextItemsExtension
{
    public static T GetValue<T>(this IDictionary<object, object?> items, string key)
    {
        if (!items.TryGetValue(key, out var value) || value is not T data)
            throw new GameException($"{key} not found");

        return data;
    }
}