using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockMemoryCache
{
    public Dictionary<object, MockCacheEntry> MockCacheEntries { get; } = new();
    public IMemoryCache Object { get; } = Substitute.For<IMemoryCache>();

    public MockMemoryCache()
    {
        Object.CreateEntry(Arg.Any<object>()).Returns(args =>
        {
            var key = args[0];
            var mockCacheEntry = new MockCacheEntry(key);
            MockCacheEntries.Add(key, mockCacheEntry);

            return mockCacheEntry;
        });

        Object.TryGetValue(Arg.Any<object>(), out Arg.Any<object?>()).Returns(args =>
            {
                var key = args[0];
                var isInCache = MockCacheEntries.TryGetValue(key, out var mockCacheEntry);
                args[1] = mockCacheEntry?.Value;
                return isInCache;
            }
        );
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
