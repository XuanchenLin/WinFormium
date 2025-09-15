// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Represents a native proxy object that can expose properties, synchronous and asynchronous functions,
/// and child objects to JavaScript. Allows dynamic definition and removal of properties and functions.
/// </summary>
public class JavaScriptNativeObject : NativeProxyObject
{
    /// <summary>
    /// Stores property getter delegates by property name.
    /// </summary>
    private readonly Dictionary<string, Func<string>> _getters = new();

    /// <summary>
    /// Stores property setter delegates by property name.
    /// </summary>
    private readonly Dictionary<string, Action<string?>> _setters = new();

    /// <summary>
    /// Stores synchronous function delegates by function name.
    /// </summary>
    private readonly Dictionary<string, Func<string?, string?>> _functions = new();

    /// <summary>
    /// Stores asynchronous function delegates by function name.
    /// </summary>
    private readonly Dictionary<string, Action<string?, JavaScriptPromiseContext>> _asyncFunctions = new();

    /// <summary>
    /// Stores child native objects by property name.
    /// </summary>
    private readonly Dictionary<string, JavaScriptNativeObject> _childObjects = new();

    /// <summary>
    /// Gets all unique property and function names defined on this object.
    /// </summary>
    private string[] Keys => _getters.Keys
        .Union(_setters.Keys)
        .Union(_functions.Keys)
        .Union(_asyncFunctions.Keys)
        .Union(_childObjects.Keys)
        .Distinct()
        .ToArray();

    /// <summary>
    /// Defines a property with a getter and optional setter.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="getter">The getter delegate returning a JSON string.</param>
    /// <param name="setter">The optional setter delegate accepting a JSON string.</param>
    /// <exception cref="ArgumentException">Thrown if the property name is already defined.</exception>
    public void DefineProperty(string name, Func<string> getter, Action<string?>? setter = null)
    {
        if (Keys.Contains(name))
            throw new ArgumentException($"Property '{name}' is already defined.", nameof(name));

        _getters[name] = getter;

        if (setter != null)
            _setters[name] = setter;
    }

    /// <summary>
    /// Removes a property by name.
    /// </summary>
    /// <param name="name">The property name to remove.</param>
    public void RemoveProperty(string name)
    {
        if (_getters.ContainsKey(name))
        {
            _getters.Remove(name);
        }
        else if (_setters.ContainsKey(name))
        {
            _setters.Remove(name);
        }
    }

    /// <summary>
    /// Defines an asynchronous function that can be called from JavaScript.
    /// </summary>
    /// <param name="name">The function name.</param>
    /// <param name="func">The function delegate accepting arguments as JSON and a <see cref="JavaScriptPromiseContext"/>.</param>
    /// <exception cref="ArgumentException">Thrown if the function name is already defined.</exception>
    public void DefineAsynchronousFunction(string name, Action<string?, JavaScriptPromiseContext> func)
    {
        if (Keys.Contains(name))
            throw new ArgumentException($"Function '{name}' is already defined.", nameof(name));

        _asyncFunctions[name] = func;
    }

    /// <summary>
    /// Defines a synchronous function that can be called from JavaScript.
    /// </summary>
    /// <param name="name">The function name.</param>
    /// <param name="func">The function delegate accepting arguments as JSON and returning a JSON string.</param>
    /// <exception cref="ArgumentException">Thrown if the function name is already defined.</exception>
    public void DefineSynchronousFunction(string name, Func<string?, string?> func)
    {
        if (Keys.Contains(name))
            throw new ArgumentException($"Function '{name}' is already defined.", nameof(name));

        _functions[name] = func;
    }

    /// <summary>
    /// Removes a function (synchronous or asynchronous) by name.
    /// </summary>
    /// <param name="name">The function name to remove.</param>
    public void RemoveFunction(string name)
    {
        if (_functions.ContainsKey(name))
        {
            _functions.Remove(name);
        }
        else if (_asyncFunctions.ContainsKey(name))
        {
            _asyncFunctions.Remove(name);
        }
    }

    /// <summary>
    /// Registers a child <see cref="JavaScriptNativeObject"/> under the specified name.
    /// </summary>
    /// <param name="name">The property name for the child object.</param>
    /// <param name="obj">The child <see cref="JavaScriptNativeObject"/> instance.</param>
    /// <exception cref="ArgumentException">Thrown if the property name is already defined.</exception>
    public void RegisterChildObject(string name, JavaScriptNativeObject obj)
    {
        if (Keys.Contains(name))
            throw new ArgumentException($"Property '{name}' is already defined.", nameof(name));
        _childObjects[name] = obj;
    }

    /// <summary>
    /// Removes a registered child object by name.
    /// </summary>
    /// <param name="name">The property name of the child object to remove.</param>
    public void RemoveChildObject(string name)
    {
        if (_childObjects.ContainsKey(name))
            _childObjects.Remove(name);
    }

    /// <summary>
    /// Handles function invocation from JavaScript, dispatching to synchronous or asynchronous functions as appropriate.
    /// </summary>
    /// <param name="args">The event arguments containing function call information.</param>
    public override void OnFunctionApply(NativeProxyObjectFunctionApplyEventArgs args)
    {
        if (_functions.TryGetValue(args.PropertyName, out var func))
        {
            var result = func(args.ArgumentsAsJson);
            args.ReturnJson(result);
        }
        else if (_asyncFunctions.TryGetValue(args.PropertyName, out var asyncFunc))
        {
            var promise = args.ReturnPromise();
            asyncFunc(args.ArgumentsAsJson, promise);
        }
    }

    /// <summary>
    /// Handles property access from JavaScript, returning property values, functions, or child objects as appropriate.
    /// </summary>
    /// <param name="args">The event arguments containing property access information.</param>
    public override void OnGettingProperty(NativeProxyObjectGetPropertyEventArgs args)
    {
        if (_getters.TryGetValue(args.PropertyName, out var getter))
        {
            var result = getter();
            args.ReturnJson(result);
        }
        else if (_functions.TryGetValue(args.PropertyName, out var func))
        {
            args.AsFunction();
        }
        else if (_asyncFunctions.TryGetValue(args.PropertyName, out var asyncFunc))
        {
            args.AsFunction();
        }
        else if (_childObjects.TryGetValue(args.PropertyName, out var childObj))
        {
            args.AsNativeObject(childObj);
        }
    }

    /// <summary>
    /// Handles property setting from JavaScript, dispatching to the appropriate setter if defined.
    /// </summary>
    /// <param name="args">The event arguments containing property setting information.</param>
    /// <returns><c>true</c> if the property was set successfully; otherwise, <c>false</c>.</returns>
    public override bool OnSettingProperty(NativeProxyObjectSetPropertyEventArgs args)
    {
        if (_setters.TryGetValue(args.PropertyName, out var setter))
        {
            setter(args.ValueAsJson);
            return true;
        }

        return false;
    }
}