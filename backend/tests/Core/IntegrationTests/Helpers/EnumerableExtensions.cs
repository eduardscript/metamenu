namespace IntegrationTests.Helpers;

public static class EnumerableExtensions
{
    public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> source, int count)
    {
        var random = new Random();
        var enumerable = source as T[] ?? source.ToArray();
        return enumerable.OrderBy(x => random.Next()).Take(count);
    }
}