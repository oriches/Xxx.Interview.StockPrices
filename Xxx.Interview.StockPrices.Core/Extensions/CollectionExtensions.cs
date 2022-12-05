namespace Xxx.Interview.StockPrices.Core.Extensions;

public static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
    {
        if (enumerable == null)
            return;

        var array = enumerable.ToArray();
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < array.Length; i++) collection.Add(array[i]);
    }
}