// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;
/// <summary>
/// Represents an abstract base class for native proxy objects that can be exposed to JavaScript.
/// Provides mechanisms for property access, property setting, and function invocation from JavaScript.
/// </summary>
public abstract class NativeProxyObject
{
    /// <summary>
    /// Stores the last used object identifier for proxy objects.
    /// </summary>
    private static volatile int _objectId = 0;

    /// <summary>
    /// Maintains a mapping of object identifiers to their corresponding <see cref="NativeProxyObject"/> instances.
    /// </summary>
    private static ConcurrentDictionary<int, NativeProxyObject> _objects = new();

    /// <summary>
    /// Retrieves a <see cref="NativeProxyObject"/> instance by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the proxy object.</param>
    /// <returns>The <see cref="NativeProxyObject"/> instance if found; otherwise, <c>null</c>.</returns>
    public static NativeProxyObject? Get(int id)
    {
        if (_objects.TryGetValue(id, out var obj))
            return obj;
        return null;
    }

    /// <summary>
    /// Gets the unique identifier for this proxy object instance.
    /// </summary>
    internal int ObjectId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NativeProxyObject"/> class and registers it in the global object dictionary.
    /// </summary>
    public NativeProxyObject()
    {
        ObjectId = Interlocked.Increment(ref _objectId);
        _objects.TryAdd(ObjectId, this);
    }

    //internal void OnGettingPropertyCore(NativeProxyObjectGetPropertyEventArgs args)
    //{
    //    OnGettingProperty(args);
    //}

    //internal bool OnSettingPropertyCore(NativeProxyObjectSetPropertyEventArgs args)
    //{
    //    return OnSettingProperty(args);
    //}

    //internal void OnFunctionApplyCore(NativeProxyObjectFunctionApplyEventArgs args)
    //{
    //    OnFunctionApply(args);
    //}

    /// <summary>
    /// Called when a property is accessed on the proxy object from JavaScript.
    /// Derived classes must implement this method to handle property access.
    /// </summary>
    /// <param name="args">The event data containing information about the property access.</param>
    public abstract void OnGettingProperty(NativeProxyObjectGetPropertyEventArgs args);

    /// <summary>
    /// Called when a property is set on the proxy object from JavaScript.
    /// Derived classes must implement this method to handle property setting.
    /// </summary>
    /// <param name="args">The event data containing information about the property being set.</param>
    /// <returns><c>true</c> if the property was set successfully; otherwise, <c>false</c>.</returns>
    public abstract bool OnSettingProperty(NativeProxyObjectSetPropertyEventArgs args);

    /// <summary>
    /// Called when a function is invoked on the proxy object from JavaScript.
    /// Derived classes must implement this method to handle function invocation.
    /// </summary>
    /// <param name="args">The event data containing information about the function call.</param>
    public abstract void OnFunctionApply(NativeProxyObjectFunctionApplyEventArgs args);

    /// <summary>
    /// Returns a JSON representation of this proxy object, typically its unique identifier.
    /// </summary>
    /// <returns>A JSON string representing the proxy object.</returns>
    internal string ToJson()
    {
        return $"{ObjectId}";
    }
}
