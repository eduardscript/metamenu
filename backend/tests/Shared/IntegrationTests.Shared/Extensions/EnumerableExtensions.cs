namespace Shared.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
    {
        var random = new Random();
        var enumerable = source as T[] ?? source.ToArray();
        return enumerable.OrderBy(x => random.Next()).Take(count);
    }
    
    public static T TakeFirstRandom<T>(this IEnumerable<T> source)
    {
        return source.TakeRandom(1).First();
    }
}