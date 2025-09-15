// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides extension methods for <see cref="HostWindowBuilder"/> to support kiosk window settings.
/// </summary>
public static class KisokWindowSettingsExtension
{
    /// <summary>
    /// Configures the builder to use kiosk window settings.
    /// </summary>
    /// <param name="builder">The <see cref="HostWindowBuilder"/> instance.</param>
    /// <returns>A new <see cref="KisokWindowSettings"/> instance for further configuration.</returns>
    public static KisokWindowSettings UseKisokWindow(this HostWindowBuilder builder)
    {
        return new KisokWindowSettings();
    }

}
