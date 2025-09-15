// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Provides an abstract base class for application startup configuration and lifecycle event handling.
/// Override this class to customize application startup, Chromium settings, and application events.
/// </summary>
public abstract class AppStartup
{
    /// <summary>
    /// Called during application startup to configure the main application action.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <param name="settings">The startup settings for the application.</param>
    /// <returns>An <see cref="AppCreationAction"/> that defines how the application should be created, or null to abort startup.</returns>
    internal abstract protected AppCreationAction? OnApplicationStartup(StartupSettings settings);

    /// <summary>
    /// Called after the application has been launched.
    /// Override to perform actions after application startup.
    /// </summary>
    /// <param name="commandArgs">The command-line arguments passed to the application.</param>
    internal protected virtual void OnApplicationLaunched(string[] commandArgs)
    {
    }

    /// <summary>
    /// Called when the application is terminating.
    /// Override to perform cleanup or finalization logic.
    /// </summary>
    internal protected virtual void OnApplicationTerminated()
    {
    }

    /// <summary>
    /// Called when an unhandled exception occurs in the application.
    /// Override to handle or log exceptions.
    /// </summary>
    /// <param name="exception">The exception that occurred, or null if not available.</param>
    internal protected virtual void OnApplicationException(Exception? exception = null)
    {
    }

    /// <summary>
    /// Allows customization of the Chromium command-line arguments before Chromium is initialized.
    /// Override to append or modify command-line switches.
    /// </summary>
    /// <param name="commandLine">The <see cref="CefCommandLine"/> instance to configure.</param>
    internal protected virtual void ConfigureChromiumCommandLine(CefCommandLine commandLine)
    {
    }

    /// <summary>
    /// Allows customization of the default Chromium settings before Chromium is initialized.
    /// Override to modify <see cref="CefSettings"/> as needed.
    /// </summary>
    /// <param name="settings">The <see cref="CefSettings"/> instance to configure.</param>
    internal protected virtual void ConfigureDefaultChromiumSettings(CefSettings settings)
    {
    }

    /// <summary>
    /// Allows customization of the default browser settings before browser creation.
    /// Override to modify <see cref="CefBrowserSettings"/> as needed.
    /// </summary>
    /// <param name="browserSettings">The <see cref="CefBrowserSettings"/> instance to configure.</param>
    internal protected virtual void ConfigureDefaultBrowserSettings(CefBrowserSettings browserSettings)
    {
    }

    /// <summary>
    /// Allows customization of the Chromium Embedded Framework redistributable settings.
    /// Override to configure redistributable detection and validation.
    /// </summary>
    /// <param name="cefRedistributableSettings">The <see cref="ChromiumEmbeddedRedistributableSettings"/> instance to configure.</param>
    internal protected virtual void ConfigureCefRedistributableSettings(ChromiumEmbeddedRedistributableSettings cefRedistributableSettings)
    {
    }

    /// <summary>
    /// Allows registration of custom Chromium URL schemes.
    /// Override to add custom schemes using the provided registrar.
    /// </summary>
    /// <param name="schemeRegistrar">The <see cref="CefSchemeRegistrar"/> instance to configure.</param>
    internal protected virtual void ConfigureSchemeRegistrar(CefSchemeRegistrar schemeRegistrar)
    {
    }

    /// <summary>
    /// Allows customization of subprocess application settings.
    /// Override to modify <see cref="SubprocessAppSettings"/> as needed.
    /// </summary>
    /// <param name="subprocessAppSettings">The <see cref="SubprocessAppSettings"/> instance to configure.</param>
    internal protected virtual void ConfigureSubprocessAppSettings(SubprocessAppSettings subprocessAppSettings)
    {
    }
}
