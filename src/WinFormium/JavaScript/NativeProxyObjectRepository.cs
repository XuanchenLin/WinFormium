// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Provides a thread-safe repository for managing <see cref="NativeProxyObject"/> instances by name.
/// </summary>
public sealed class NativeProxyObjectRepository
{
    /// <summary>
    /// Stores the registered <see cref="NativeProxyObject"/> instances, keyed by their unique names.
    /// </summary>
    private readonly ConcurrentDictionary<string, NativeProxyObject> _objects = new();

    /// <summary>
    /// Retrieves a registered <see cref="NativeProxyObject"/> by its name.
    /// </summary>
    /// <param name="name">The unique name of the object to retrieve.</param>
    /// <returns>
    /// The <see cref="NativeProxyObject"/> instance if found; otherwise, <c>null</c>.
    /// </returns>
    public NativeProxyObject? Get(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        if (_objects.TryGetValue(name, out var obj))
            return obj;
        return null;
    }

    /// <summary>
    /// Registers a new <see cref="NativeProxyObject"/> with the specified name.
    /// </summary>
    /// <param name="key">The unique name to associate with the object.</param>
    /// <param name="obj">The <see cref="NativeProxyObject"/> instance to register.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if an object with the same name is already registered.</exception>
    public void Register(string key, NativeProxyObject obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(NativeProxyObject));

        if (_objects.Any(x => x.Key == key))
            throw new ArgumentException($"An object with the name '{key}' is already registered.", nameof(obj));

        _objects.TryAdd(key, obj);
    }

    /// <summary>
    /// Unregisters the <see cref="NativeProxyObject"/> associated with the specified name.
    /// </summary>
    /// <param name="name">The unique name of the object to unregister.</param>
    public void Unregister(string name)
    {
        if (_objects.TryRemove(name, out var obj))
        {

        }
    }
}
