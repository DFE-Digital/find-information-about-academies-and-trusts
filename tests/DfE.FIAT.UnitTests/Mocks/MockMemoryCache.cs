using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DfE.FIAT.UnitTests.Mocks;

public class MockMemoryCache : Mock<IMemoryCache>
{
    public Dictionary<object, MockCacheEntry> MockCacheEntries { get; } = new();

    public MockMemoryCache()
    {
        Setup(m => m.CreateEntry(It.IsAny<object>())).Returns((object key) =>
        {
            var mockCacheEntry = new MockCacheEntry(key);
            MockCacheEntries.Add(key, mockCacheEntry);

            return mockCacheEntry;
        });

        object? outReturn; //needed separately by Moq for out parameter
        Setup(m => m.TryGetValue(It.IsAny<object>(), out outReturn))
            .Returns((object key, out object? value) =>
            {
                var isInCache = MockCacheEntries.TryGetValue(key, out var mockCacheEntry);
                value = mockCacheEntry?.Value;
                return isInCache;
            });
    }

    public void AddMockCacheEntry<TItem>(object key, TItem value)
    {
        var mockCacheEntry = new MockCacheEntry(key)
            { Value = value, AbsoluteExpirationRelativeToNow = TimeSpan.MaxValue };
        MockCacheEntries.Add(key, mockCacheEntry);
    }
}

public class MockCacheEntry(object key) : ICacheEntry
{
    public object Key { get; } = key;
    public object? Value { get; set; }
    public DateTimeOffset? AbsoluteExpiration { get; set; }
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
    public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();

    public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } =
        new List<PostEvictionCallbackRegistration>();

    public CacheItemPriority Priority { get; set; }
    public long? Size { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
