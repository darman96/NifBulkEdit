namespace NifBulkEdit.Core.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> func)
    {
        foreach (var item in source)
        {
            func(item);
        }
    }

    public static bool None<TSource>(this IEnumerable<TSource> source)
        => !source.Any();
}