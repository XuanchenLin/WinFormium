// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Represents the environment configuration and settings for the Chromium Embedded Framework (CEF) within WinFormium.
/// </summary>
public sealed class ChromiumEmbeddedEnvironment
{
    /// <summary>
    /// Gets the settings for the Chromium Embedded redistributable.
    /// </summary>
    internal ChromiumEmbeddedRedistributableSettings CefRedistributableSettings { get; }
    private readonly ChromiumEmbeddedEnvironmentBulider _builder;

    /// <summary>
    /// Gets or sets the action to configure the CEF command line.
    /// </summary>
    internal Action<CefCommandLine>? ConfigureCommandLine { get; set; }
    /// <summary>
    /// Gets or sets the action to configure the default Chromium settings.
    /// </summary>
    internal Action<CefSettings>? ConfigureDefaultChromiumSettings { get; set; }
    /// <summary>
    /// Gets or sets the action to configure the default browser settings.
    /// </summary>
    internal Action<CefBrowserSettings>? ConfigureDefaultBrowserSettings { get; set; }
    /// <summary>
    /// Gets or sets the action to configure the Chromium Embedded redistributable.
    /// </summary>
    internal Action<ChromiumEmbeddedRedistributableSettings>? ConfigureChromiumEmbeddedRedistributable { get; set; }
    /// <summary>
    /// Gets or sets the action to configure the scheme registrar.
    /// </summary>
    internal Action<CefSchemeRegistrar>? ConfigureSchemeRegistrar { get; set; }
    /// <summary>
    /// Gets or sets the action to configure the subprocess application settings.
    /// </summary>
    internal Action<SubprocessAppSettings>? ConfigureSubprocessAppSettings { get; set; }

    /// <summary>
    /// Gets the directory path of the libcef library.
    /// </summary>
    public string LibCefDirPath => CefRedistributableSettings.LibCefDirPath;
    /// <summary>
    /// Gets the directory path of the CEF resources.
    /// </summary>
    public string ResourceDirPath => CefRedistributableSettings.ResourceDirPath;
    /// <summary>
    /// Gets the directory path of the CEF locales.
    /// </summary>
    public string LocaleDirPath => CefRedistributableSettings.LocaleDirPath;

    /// <summary>
    /// Gets the application data directory.
    /// </summary>
    public string AppDataDirectory { get; private set; }
    /// <summary>
    /// Gets the cache directory path.
    /// </summary>
    public string CacheDiretory => Path.Combine(AppDataDirectory, "Cache");
    /// <summary>
    /// Gets the user data directory path.
    /// </summary>
    public string UserDataDirectory => Path.Combine(AppDataDirectory, "UserData");

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromiumEmbeddedEnvironment"/> class.
    /// </summary>
    /// <param name="builder">The environment builder used for configuration.</param>
    internal ChromiumEmbeddedEnvironment(ChromiumEmbeddedEnvironmentBulider builder)
    {
        _builder = builder;
        CefRedistributableSettings = new ChromiumEmbeddedRedistributableSettings(_builder.AppBuilder.PlatformArchitecture, ChromiumEmbeddedEnvironmentBulider.ChromiumVersion);
        AppDataDirectory = _builder.AppBuilder.AppDataDirectory;
    }

    /// <summary>
    /// Creates the default CEF settings, ensuring the application data directory exists.
    /// </summary>
    /// <returns>A <see cref="CefSettings"/> instance with default values.</returns>
    internal CefSettings CreateDefaultSettings()
    {

    CreateAppDataDir:
        if (!Directory.Exists(AppDataDirectory))
        {

            try
            {
                Directory.CreateDirectory(AppDataDirectory);

            }
            catch (IOException)
            {
                WinFormMessageBox.Show(string.Format("Failed to create the application data directory on {0}, the default path will be used.", AppDataDirectory), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                AppDataDirectory = _builder.AppBuilder.DefaultAppDataDirectory;

                goto CreateAppDataDir;
            }
        }

        return new CefSettings
        {
            LogFile = Path.Combine(AppDataDirectory, "winformium_cef_debug.log"),
            LogSeverity = CefLogSeverity.Fatal,
            JavaScriptFlags = "--expose-gc,--optimize_for_size",
            PersistSessionCookies = true,
            PersistUserPreferences = true,
            BackgroundColor = new CefColor(255, 255, 255, 255),
        };
    }

    /// <summary>
    /// Creates the default browser settings for CEF.
    /// </summary>
    /// <returns>A <see cref="CefBrowserSettings"/> instance with default values.</returns>
    internal CefBrowserSettings CreateDefaultBrowserSettings()
    {
        var culure = _builder.AppBuilder.Culture.Name;

        return new CefBrowserSettings
        {
            BackgroundColor = new CefColor(255, 255, 255, 255),
            WindowlessFrameRate = 60,
            DefaultEncoding = "UTF-8",
            JavaScriptCloseWindows = CefState.Enabled,
            JavaScriptAccessClipboard = CefState.Enabled,
        };
    }
}
