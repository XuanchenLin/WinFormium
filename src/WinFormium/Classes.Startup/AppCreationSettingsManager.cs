// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Manages application creation settings using a key-value store.
/// </summary>
public sealed class AppCreationSettingsManager
{
    /// <summary>
    /// Stores the settings with their associated names.
    /// </summary>
    readonly Dictionary<string, AppCreationSetting> settings = new();

    /// <summary>
    /// Sets the value for the specified setting name.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <param name="value">The value to set.</param>
    public void SetValue(string name, object value)
    {
        settings[name] = new AppCreationSetting(value, value.GetType());
    }

    /// <summary>
    /// Sets the value for the specified setting name with a generic type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="name">The name of the setting.</param>
    /// <param name="value">The value to set.</param>
    public void SetValue<T>(string name, T value) where T : notnull
    {
        settings[name] = new AppCreationSetting(value, typeof(T));
    }

    /// <summary>
    /// Gets the value of the specified setting name.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <returns>The value of the setting if found; otherwise, null.</returns>
    public object? GetValue(string name)
    {
        if (settings.TryGetValue(name, out var setting))
        {
            return setting.Value;
        }

        return null;
    }

    /// <summary>
    /// Gets the <see cref="AppCreationSetting"/> for the specified name.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <returns>The <see cref="AppCreationSetting"/> associated with the name.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the setting does not exist.</exception>
    public AppCreationSetting GetProperty(string name)
    {
        if (settings.TryGetValue(name, out var setting))
        {
            return setting;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Gets the value of the specified setting name with a generic type.
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="name">The name of the setting.</param>
    /// <returns>The value of the setting cast to type <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the setting does not exist.</exception>
    public T GetValue<T>(string name)
    {
        if (settings.TryGetValue(name, out var setting))
        {
            return (T)setting.Value;
        }

        throw new KeyNotFoundException($"Key Name:{name}");
    }

    /// <summary>
    /// Removes the setting with the specified name.
    /// </summary>
    /// <param name="name">The name of the setting to remove.</param>
    public void Remove(string name)
    {
        settings.Remove(name);
    }

    /// <summary>
    /// Determines whether a setting with the specified name exists.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <returns><c>true</c> if the setting exists; otherwise, <c>false</c>.</returns>
    public bool Exists(string name)
    {
        return settings.ContainsKey(name);
    }

}

