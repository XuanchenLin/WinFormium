// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides a builder for creating and configuring host windows in WinFormium.
/// </summary>
public sealed class HostWindowBuilder
{
    /// <summary>
    /// Configures the builder to use the default window settings.
    /// </summary>
    /// <returns>
    /// A <see cref="DefaultWindowSettings"/> instance for further configuration.
    /// </returns>
    public DefaultWindowSettings UseDefaultWindow()
    {
        return new DefaultWindowSettings();
    }
}
