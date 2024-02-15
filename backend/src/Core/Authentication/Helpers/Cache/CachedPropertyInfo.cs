using System.Collections.Concurrent;
using System.Reflection;

namespace Core.Authentication.Helpers.Cache;

public class CachedPropertyInfo
{
    public PropertyInfo PropertyInfo { get; set; } = default!;

    public IEnumerable<Attribute> Attributes { get; set; } = default!;
}

public interface IPropertyCache
{
    IEnumerable<CachedPropertyInfo> GetCachedProperties(Type type);
}

public class PropertyCache : IPropertyCache
{
    private readonly ConcurrentDictionary<Type, IEnumerable<CachedPropertyInfo>> _propertyCache = new();


    public IEnumerable<CachedPropertyInfo> GetCachedProperties(Type type)
    {
        if (!_propertyCache.TryGetValue(type, out var cachedPropertyInfos))
        {
            cachedPropertyInfos = type.GetProperties().Select(p => new CachedPropertyInfo
            {
                PropertyInfo = p,
                Attributes = p.GetCustomAttributes(true).OfType<Attribute>()
            }).ToList();
            
            _propertyCache.TryAdd(type, cachedPropertyInfos);
        }
        
        return cachedPropertyInfos;
    }
}