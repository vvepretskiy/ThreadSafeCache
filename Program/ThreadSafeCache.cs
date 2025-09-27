using System.Collections.Concurrent;

public class ThreadSafeCache<TKey, TValue> : IDisposable where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _cache;
    private bool _disposed;

    public ThreadSafeCache()
    {
        _cache = new ConcurrentDictionary<TKey, Lazy<TValue>>();
    }

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ThreadSafeCache<TKey, TValue>));

        // Use Lazy<TValue> to ensure that the value is created only once
        var lazyValue = _cache.GetOrAdd(key, k => new Lazy<TValue>(() => valueFactory(k)));
        return lazyValue.Value;
    }

    public bool TryRemove(TKey key, out TValue value)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ThreadSafeCache<TKey, TValue>));

        if (_cache.TryRemove(key, out var lazyValue))
        {
            value = lazyValue.Value;
            return true;
        }

        value = default!;
        return false;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _cache.Clear();
            }

            _disposed = true;
        }
    }
}