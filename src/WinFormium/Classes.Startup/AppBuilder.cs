// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides a builder for configuring and creating a <see cref="WinFormiumApp"/> instance.
/// </summary>
public sealed class AppBuilder
{
    /// <summary>
    /// The prefix used to identify the process type argument in command line arguments.
    /// </summary>
    private const string TYPE_ARG_PREFIX = "--type=";

    /// <summary>
    /// Gets the internal settings manager for application creation.
    /// </summary>
    internal AppCreationSettingsManager InternalSettings { get; } = new AppCreationSettingsManager();

    /// <summary>
    /// Gets the platform architecture (x86 or x64) of the current process.
    /// </summary>
    internal PlatformArchitecture PlatformArchitecture { get; }

    /// <summary>
    /// Gets the process type (Browser or Renderer) for the current process.
    /// </summary>
    internal CefProcessId ProcessType { get; }

    /// <summary>
    /// Gets the application data directory path.
    /// </summary>
    internal string AppDataDirectory { get => InternalSettings.GetValue<string>(name: nameof(AppDataDirectory)); }

    /// <summary>
    /// Gets the culture information for the application.
    /// </summary>
    internal CultureInfo Culture { get => new(InternalSettings.GetValue<string>(nameof(Culture))); }

    /// <summary>
    /// Gets a value indicating whether the DevTools menu is enabled.
    /// </summary>
    internal bool EnableDevToolsMenu { get => InternalSettings.GetValue<bool>(nameof(EnableDevToolsMenu)); }

    /// <summary>
    /// Gets a value indicating whether the embedded browser is enabled.
    /// </summary>
    internal bool EnableEmbeddedBrowser { get => InternalSettings.GetValue<bool>(nameof(EnableEmbeddedBrowser)); }

    /// <summary>
    /// Gets a value indicating whether single instance mode is enabled.
    /// </summary>
    internal bool EnableSingleInstanceMode { get => InternalSettings.GetValue<bool>(nameof(EnableSingleInstanceMode)); }

    /// <summary>
    /// Gets the builder for configuring the Chromium Embedded Environment.
    /// </summary>
    internal ChromiumEmbeddedEnvironmentBulider ChromiumeEmbeddedEnvironmentBuilder { get; }

    /// <summary>
    /// Gets the application startup instance.
    /// </summary>
    internal AppStartup? AppStartup { get; private set; }

    private Action<ChromiumEmbeddedEnvironmentBulider>? _configureChromiumEmbeddedEnvironment;

    /// <summary>
    /// Gets the Chromium embedded environment instance.
    /// </summary>
    internal ChromiumEmbeddedEnvironment ChromiumEmbeddedEnvironment { get => InternalSettings.GetValue<ChromiumEmbeddedEnvironment>(nameof(ChromiumEmbeddedEnvironment)); }

    /// <summary>
    /// Gets the list of resource scheme handler factories.
    /// </summary>
    internal List<Func<WebResourceSchemeHandlerFactory>> ResourceSchemeHandlerFactories { get; } = new();

    /// <summary>
    /// Gets the default application data directory path.
    /// </summary>
    internal string DefaultAppDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.ProductName ?? $"WinFormiumApp_{$"{Guid.NewGuid()}".Replace("-", string.Empty).ToLower()}");

    /// <summary>
    /// Gets the list of actions to be executed when WebKit is initialized in the render process.
    /// </summary>
    internal List<Action> WebKitInitializedActions { get; } = new List<Action>();


    /// <summary>
    /// Gets or sets a value indicating whether the cache should be cleaned up.
    /// </summary>
    internal bool CacheShouldCleaned { get; set; } = false;

    /// <summary>
    /// Gets or sets the system color mode.
    /// </summary>
    internal SystemColorMode SystemColorMode { get; set; } = SystemColorMode.Auto;

    /// <summary>
    /// Gets the public application creation settings manager.
    /// </summary>
    public AppCreationSettingsManager CreationSettings { get; } = new AppCreationSettingsManager();

    /// <summary>
    /// Initializes a new instance of the <see cref="AppBuilder"/> class.
    /// </summary>
    internal AppBuilder()
    {
        var args = Environment.GetCommandLineArgs();

        PlatformArchitecture = IntPtr.Size switch
        {
            4 => PlatformArchitecture.x86,
            8 => PlatformArchitecture.x64,
            _ => throw new Exception("Unknown architecture")
        };

        InternalSettings.SetValue(nameof(PlatformArchitecture), PlatformArchitecture);

        ProcessType = args.FirstOrDefault(x => x.StartsWith(TYPE_ARG_PREFIX)) == null ? CefProcessId.Browser : CefProcessId.Renderer;

        InternalSettings.SetValue(nameof(ProcessType), ProcessType);

        InternalSettings.SetValue(nameof(AppDataDirectory), DefaultAppDataDirectory);

        InternalSettings.SetValue(nameof(Culture), CultureInfo.CurrentCulture.Name);

        InternalSettings.SetValue(nameof(EnableDevToolsMenu), false);

        InternalSettings.SetValue(nameof(EnableEmbeddedBrowser), false);

        InternalSettings.SetValue(nameof(EnableSingleInstanceMode), true);

        InternalSettings.SetValue(nameof(SystemColorMode), SystemColorMode);

        ChromiumeEmbeddedEnvironmentBuilder = new ChromiumEmbeddedEnvironmentBulider(this);
    }

    /// <summary>
    /// Sets a custom application data directory.
    /// </summary>
    /// <param name="directory">The directory path to use.</param>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder UseCustomAppDataDirectory(string directory)
    {
        InternalSettings.SetValue(nameof(AppDataDirectory), directory);
        return this;
    }

    /// <summary>
    /// Sets the culture for the application.
    /// </summary>
    /// <param name="culture">The culture name (e.g., "en-US").</param>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder UseCulture(string culture)
    {
        InternalSettings.SetValue(nameof(Culture), culture);
        return this;
    }

    /// <summary>
    /// Enables the DevTools menu.
    /// </summary>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder UseDevTools()
    {
        InternalSettings.SetValue(nameof(EnableDevToolsMenu), true);
        return this;
    }

    /// <summary>
    /// Enables the embedded browser.
    /// </summary>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder UseEmbeddedBrowser()
    {
        InternalSettings.SetValue(nameof(EnableEmbeddedBrowser), true);
        return this;
    }

    /// <summary>
    /// Enables single running instance mode and optionally sets a handler for when an instance is already running.
    /// </summary>
    /// <param name="instanceIsRunnningHandler">The handler to invoke if an instance is already running.</param>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder UseSingleRunningInstance(HandleInstanceIsRunningDelegate? instanceIsRunnningHandler = null)
    {
        InternalSettings.SetValue(nameof(EnableSingleInstanceMode), true);
        if (instanceIsRunnningHandler != null)
        {
            InternalSettings.SetValue(nameof(UseSingleRunningInstance), instanceIsRunnningHandler);
        }
        return this;
    }

    /// <summary>
    /// Configures the Chromium Embedded Environment using the specified action.
    /// </summary>
    /// <param name="configureChromiumEmbeddedEnvironment">The configuration action.</param>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder ConfigureChromiumEmbeddedEnvironment(Action<ChromiumEmbeddedEnvironmentBulider> configureChromiumEmbeddedEnvironment)
    {
        _configureChromiumEmbeddedEnvironment += configureChromiumEmbeddedEnvironment;
        return this;
    }

    /// <summary>
    /// Enables cache cleanup on application startup.
    /// </summary>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder UseCacheCleanup()
    {
        CacheShouldCleaned = true;
        return this;
    }

    /// <summary>
    /// Adds a resource scheme handler factory to the application.
    /// </summary>
    /// <param name="factory">The factory function to add.</param>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder AddResourceSchemeHandlerFactory(Func<WebResourceSchemeHandlerFactory> factory)
    {
        if (ProcessType == CefProcessId.Browser)
        {
            ResourceSchemeHandlerFactories.Add(factory);
        }
        return this;
    }

    /// <summary>
    /// Sets the application startup class to use.
    /// </summary>
    /// <typeparam name="TApp">The type of the application startup class.</typeparam>
    /// <returns>The current <see cref="AppBuilder"/> instance.</returns>
    public AppBuilder UseWinFormiumApp<TApp>() where TApp : AppStartup, new()
    {
        if (ProcessType == CefProcessId.Browser)
        {
            var startup = Activator.CreateInstance<TApp>();
            InternalSettings.SetValue(nameof(WinFormiumApp), startup);
        }
        return this;
    }

    /// <summary>
    /// Adds a script to be executed when the WebKit is initialized in the renderer process.
    /// </summary>
    /// <param name="initScriptAction">
    /// The action to execute when WebKit is initialized.
    /// </param>
    /// <returns></returns>
    public AppBuilder AddWindowBindingScripts(Action initScriptAction)
    {
        if(ProcessType == CefProcessId.Renderer)
        {
            WebKitInitializedActions.Add(initScriptAction);
        }
        return this;
    }

    /// <summary>
    /// Builds and returns a <see cref="WinFormiumApp"/> instance using the current configuration.
    /// </summary>
    /// <returns>The created <see cref="WinFormiumApp"/> instance.</returns>
    public WinFormiumApp Build()
    {
        var app = new WinFormiumApp(this);

        if (ProcessType == CefProcessId.Browser)
        {
            BuildActionOnMainProcess(app);
        }
        else
        {
            BuildActionOnRendererProcess(app);
        }

        var env = ChromiumeEmbeddedEnvironmentBuilder.Build();

        InternalSettings.SetValue(nameof(ChromiumEmbeddedEnvironment), env);

        WinFormiumApp.Current = app;

        return app;
    }

    /// <summary>
    /// Performs main process-specific build actions.
    /// </summary>
    /// <param name="app">The <see cref="WinFormiumApp"/> instance.</param>
    private void BuildActionOnMainProcess(WinFormiumApp app)
    {
        AppStartup = app.AppBuilder.InternalSettings.GetValue<AppStartup>(nameof(WinFormiumApp));

        _configureChromiumEmbeddedEnvironment?.Invoke(ChromiumeEmbeddedEnvironmentBuilder);

        if (AppStartup != null)
        {
            ChromiumeEmbeddedEnvironmentBuilder.ConfigureCommandLine(AppStartup.ConfigureChromiumCommandLine);
            ChromiumeEmbeddedEnvironmentBuilder.ConfigureDefaultChromiumSettings(AppStartup.ConfigureDefaultChromiumSettings);
            ChromiumeEmbeddedEnvironmentBuilder.ConfigureDefaultBrowserSettings(AppStartup.ConfigureDefaultBrowserSettings);
            ChromiumeEmbeddedEnvironmentBuilder.ConfigureChromiumEmbeddedRedistributable(AppStartup.ConfigureCefRedistributableSettings);
            ChromiumeEmbeddedEnvironmentBuilder.ConfigureSubprocessAppSettings(AppStartup.ConfigureSubprocessAppSettings);
        }
    }

    /// <summary>
    /// Performs renderer process-specific build actions.
    /// </summary>
    /// <param name="app">The <see cref="WinFormiumApp"/> instance.</param>
    private void BuildActionOnRendererProcess(WinFormiumApp app)
    {

    }
}


