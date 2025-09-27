using var cache = new ThreadSafeCache<string, string>();

// Adding items to the cache
var value1 = cache.GetOrAdd("key1", key => "Value for " + key);
Console.WriteLine(value1); // Output: Value for key1

// Retrieving the same item from the cache
var value2 = cache.GetOrAdd("key1", key => "This won't be called");
Console.WriteLine(value2); // Output: Value for key1

// Removing an item from the cache
if (cache.TryRemove("key1", out var removedValue))
{
    Console.WriteLine($"Removed: {removedValue}"); // Output: Removed: Value for key1
}

Console.ReadKey();