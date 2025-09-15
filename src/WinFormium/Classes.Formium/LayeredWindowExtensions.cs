// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides extension methods for <see cref="HostWindowBuilder"/> to support layered window creation.
/// </summary>
public static class LayeredWindowExtensions
{
    /// <summary>
    /// Configures the builder to use layered window settings, enabling advanced window features
    /// such as per-pixel transparency and custom rendering.
    /// </summary>
    /// <param name="builder">The <see cref="HostWindowBuilder"/> instance to extend.</param>
    /// <returns>
    /// A <see cref="LayeredWindowSettings"/> instance for further configuration.
    /// </returns>
    public static LayeredWindowSettings UseLayeredWindow(this HostWindowBuilder builder)
    {
        var settings = new LayeredWindowSettings();
        return settings;
    }
}
