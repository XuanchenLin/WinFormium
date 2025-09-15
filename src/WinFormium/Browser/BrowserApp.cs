// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Represents the main application class for the embedded Chromium browser.
/// Handles process handlers, command line processing, and custom scheme registration.
/// </summary>
class BrowserApp : CefApp
{
    /// <summary>
    /// Gets the application context for WinFormium.
    /// </summary>
    public WinFormiumApp AppContext { get; }

    /// <summary>
    /// Gets the Chromium embedded environment associated with the application context.
    /// </summary>
    public ChromiumEmbeddedEnvironment ChromiumEmbeddedEnvironment => AppContext.ChromiumEmbeddedEnvironment;

    BrowserProcessHandler? _browserProcessHandler;
    RenderProcessHandler? _renderProcessHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserApp"/> class.
    /// </summary>
    /// <param name="app">The WinFormium application context.</param>
    public BrowserApp(WinFormiumApp app)
    {
        AppContext = app;
    }

    /// <summary>
    /// Gets the process type name from the command line arguments.
    /// </summary>
    /// <returns>The process type name, or "browser" if not specified.</returns>
    public string GetProcessTypeName()
    {
        var args = Environment.GetCommandLineArgs();

        var processTypeArg = args.FirstOrDefault(x => x.StartsWith("--type", StringComparison.CurrentCultureIgnoreCase));

        var argv = string.IsNullOrEmpty(processTypeArg) ? null : Regex.Replace(processTypeArg, "--type=", string.Empty, RegexOptions.IgnoreCase);

        return argv ?? "browser";
    }

    /// <summary>
    /// Returns the handler for functionality specific to the browser process.
    /// </summary>
    /// <returns>The <see cref="CefBrowserProcessHandler"/> instance.</returns>
    protected override CefBrowserProcessHandler GetBrowserProcessHandler()
    {
        if (_browserProcessHandler == null)
        {
            _browserProcessHandler = new BrowserProcessHandler(this);
        }
        return _browserProcessHandler;
    }

    /// <summary>
    /// Returns the handler for functionality specific to the render process.
    /// </summary>
    /// <returns>The <see cref="CefRenderProcessHandler"/> instance.</returns>
    protected override CefRenderProcessHandler GetRenderProcessHandler()
    {
        if (_renderProcessHandler == null)
        {
            _renderProcessHandler = new RenderProcessHandler(this);
        }
        return _renderProcessHandler;
    }

    /// <summary>
    /// Provides an opportunity to view and/or modify command-line arguments before processing by CEF and Chromium.
    /// </summary>
    /// <param name="processType">The process type.</param>
    /// <param name="commandLine">The command line object.</param>
    protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
    {
        base.OnBeforeCommandLineProcessing(processType, commandLine);

        commandLine.AppendSwitch("ignore-certificate-errors");
        commandLine.AppendSwitch("enable-media-stream");
        //commandLine.AppendSwitch("disable-gpu");
        //commandLine.AppendSwitch("disable-gpu-compositing");
        commandLine.AppendSwitch("autoplay-policy", "no-user-gesture-required");
        commandLine.AppendSwitch("top-chrome-md", "material");
        commandLine.AppendSwitch("renderer-process-limit", "1");
        //commandLine.AppendSwitch("in-process-gpu");
        //commandLine.AppendSwitch("enable-low-end-device-mode");
        //commandLine.AppendSwitch("enable-features", "NetworkServiceInProcess,StorageServiceInProcess,AudioServiceInProcess,TracingServiceInProcess");

        ChromiumEmbeddedEnvironment.ConfigureCommandLine?.Invoke(commandLine);

        commandLine.AppendSwitch("user-agent-product", $"Chromium/{CefRuntime.ChromeVersion} WinFormium/{Assembly.GetExecutingAssembly().GetName().Version}");
        commandLine.AppendSwitch("in-process-gpu");
        commandLine.AppendSwitch("in-process-broker");
        commandLine.AppendSwitch("force-device-scale", "1");

    }

    /// <summary>
    /// Registers custom schemes for the application.
    /// </summary>
    /// <param name="registrar">The scheme registrar.</param>
    protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
    {
        base.OnRegisterCustomSchemes(registrar);

        ChromiumEmbeddedEnvironment.ConfigureSchemeRegistrar?.Invoke(registrar);

        registrar.AddCustomScheme("formium", CefSchemeOptions.Secure | CefSchemeOptions.Standard);
    }

    /// <summary>
    /// Returns the resource bundle handler for the application.
    /// </summary>
    /// <returns>The <see cref="CefResourceBundleHandler"/> instance.</returns>
    protected override CefResourceBundleHandler GetResourceBundleHandler()
    {
        return base.GetResourceBundleHandler();
    }
}
