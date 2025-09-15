// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides a builder for configuring and creating a <see cref="ChromiumEmbeddedEnvironment"/> instance.
/// </summary>
public sealed class ChromiumEmbeddedEnvironmentBulider
{
    /// <summary>
    /// The version of the embedded Chromium runtime.
    /// </summary>
    internal const string ChromiumVersion = "109.0.5414";

    /// <summary>
    /// Gets the associated <see cref="AppBuilder"/> instance.
    /// </summary>
    internal AppBuilder AppBuilder { get; }

    /// <summary>
    /// Gets the directory where the application is running.
    /// </summary>
    public static string ApplicationRunningDirectory => System.AppContext.BaseDirectory;

    private Action<CefCommandLine>? _configureCommandLine;
    private Action<CefSettings>? _configureSettings;
    private Action<CefBrowserSettings>? _configureBrowserSettings;
    private Action<ChromiumEmbeddedRedistributableSettings>? _configureCefRedistributableSettings;
    private Action<CefSchemeRegistrar>? _configureSchemeRegistrar;
    private Action<SubprocessAppSettings>? _configureSubprocessAppSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromiumEmbeddedEnvironmentBulider"/> class.
    /// </summary>
    /// <param name="appBuilder">The <see cref="AppBuilder"/> to associate with this builder.</param>
    internal ChromiumEmbeddedEnvironmentBulider(AppBuilder appBuilder)
    {
        AppBuilder = appBuilder;
    }

    /// <summary>
    /// Configures the Chromium command line arguments.
    /// </summary>
    /// <param name="configureCommandLine">An action to configure the <see cref="CefCommandLine"/>.</param>
    /// <returns>The current <see cref="ChromiumEmbeddedEnvironmentBulider"/> instance.</returns>
    public ChromiumEmbeddedEnvironmentBulider ConfigureCommandLine(Action<CefCommandLine> configureCommandLine)
    {
        _configureCommandLine += configureCommandLine;
        return this;
    }

    /// <summary>
    /// Configures the default Chromium settings.
    /// </summary>
    /// <param name="configureSettings">An action to configure the <see cref="CefSettings"/>.</param>
    /// <returns>The current <see cref="ChromiumEmbeddedEnvironmentBulider"/> instance.</returns>
    public ChromiumEmbeddedEnvironmentBulider ConfigureDefaultChromiumSettings(Action<CefSettings> configureSettings)
    {
        _configureSettings += configureSettings;
        return this;
    }

    /// <summary>
    /// Configures the default browser settings.
    /// </summary>
    /// <param name="configureBrowserSettings">An action to configure the <see cref="CefBrowserSettings"/>.</param>
    /// <returns>The current <see cref="ChromiumEmbeddedEnvironmentBulider"/> instance.</returns>
    public ChromiumEmbeddedEnvironmentBulider ConfigureDefaultBrowserSettings(Action<CefBrowserSettings> configureBrowserSettings)
    {
        _configureBrowserSettings += configureBrowserSettings;
        return this;
    }

    /// <summary>
    /// Configures the Chromium Embedded redistributable settings.
    /// </summary>
    /// <param name="configureCefRedistributable">An action to configure the <see cref="ChromiumEmbeddedRedistributableSettings"/>.</param>
    /// <returns>The current <see cref="ChromiumEmbeddedEnvironmentBulider"/> instance.</returns>
    public ChromiumEmbeddedEnvironmentBulider ConfigureChromiumEmbeddedRedistributable(Action<ChromiumEmbeddedRedistributableSettings> configureCefRedistributable)
    {
        _configureCefRedistributableSettings += configureCefRedistributable;
        return this;
    }

    /// <summary>
    /// Configures the scheme registrar for custom Chromium URL schemes.
    /// </summary>
    /// <param name="configureSchemeRegistrar">An action to configure the <see cref="CefSchemeRegistrar"/>.</param>
    /// <returns>The current <see cref="ChromiumEmbeddedEnvironmentBulider"/> instance.</returns>
    public ChromiumEmbeddedEnvironmentBulider ConfigureSchemeRegistrar(Action<CefSchemeRegistrar> configureSchemeRegistrar)
    {
        _configureSchemeRegistrar += configureSchemeRegistrar;
        return this;
    }

    /// <summary>
    /// Configures the subprocess application settings.
    /// </summary>
    /// <param name="configureSubprocessAppSettings">An action to configure the <see cref="SubprocessAppSettings"/>.</param>
    /// <returns>The current <see cref="ChromiumEmbeddedEnvironmentBulider"/> instance.</returns>
    public ChromiumEmbeddedEnvironmentBulider ConfigureSubprocessAppSettings(Action<SubprocessAppSettings> configureSubprocessAppSettings)
    {
        _configureSubprocessAppSettings += configureSubprocessAppSettings;
        return this;
    }

    /// <summary>
    /// Builds and returns a <see cref="ChromiumEmbeddedEnvironment"/> instance using the current configuration.
    /// </summary>
    /// <returns>The created <see cref="ChromiumEmbeddedEnvironment"/> instance.</returns>
    internal ChromiumEmbeddedEnvironment Build()
    {
        var env = new ChromiumEmbeddedEnvironment(this)
        {
            ConfigureCommandLine = _configureCommandLine,
            ConfigureDefaultChromiumSettings = _configureSettings,
            ConfigureDefaultBrowserSettings = _configureBrowserSettings,
            ConfigureChromiumEmbeddedRedistributable = _configureCefRedistributableSettings,
            ConfigureSchemeRegistrar = _configureSchemeRegistrar,
            ConfigureSubprocessAppSettings = _configureSubprocessAppSettings
        };

        return env;
    }
}
