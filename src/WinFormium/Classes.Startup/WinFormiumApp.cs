// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

using WinFormium.Browser.ProcessCommunication.NamedPipe;

namespace WinFormium;


/// <summary>
/// Represents the main application class for WinFormium, managing application lifecycle, process communication,
/// Chromium Embedded Framework (CEF) initialization, and environment configuration.
/// </summary>
public sealed class WinFormiumApp
{
    /// <summary>
    /// Gets the current <see cref="WinFormiumApp"/> instance.
    /// </summary>
    public static WinFormiumApp Current
    {
        get
        {
            if (_current == null)
            {
                throw new InvalidOperationException("WinFormiumApp has not been created.");
            }

            return _current;
        }
        internal set => _current = value;
    }

    /// <summary>
    /// Gets the process ID of the browser process.
    /// </summary>
    public int BrowserProcessId { get; }

    /// <summary>
    /// Gets the platform architecture of the application.
    /// </summary>
    public PlatformArchitecture Architecture
    {
        get
        {
            if (_builder == null)
            {
                throw new InvalidOperationException("WinFormiumAppBuilder has not been created.");
            }


            return _builder.PlatformArchitecture;
        }
    }

    /// <summary>
    /// Gets the type of the current CEF process.
    /// </summary>
    public CefProcessId ProcessType
    {
        get
        {
            if (_builder == null)
            {
                throw new InvalidOperationException("WinFormiumAppBuilder has not been created.");
            }
            return _builder.ProcessType;
        }
    }

    /// <summary>
    /// Gets the culture used by the application.
    /// </summary>
    public CultureInfo Culture
    {
        get
        {
            return AppBuilder.Culture;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the DevTools menu is enabled.
    /// </summary>
    public bool EnableDevToolsMenu
    {
        get
        {
            return AppBuilder.EnableDevToolsMenu;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the embedded browser is enabled.
    /// </summary>
    public bool EnableEmbeddedBrowser
    {
        get
        {
            return AppBuilder.EnableEmbeddedBrowser;
        }
    }

    /// <summary>
    /// Gets a value indicating whether single instance mode is enabled.
    /// </summary>
    public bool SingleInstanceMode
    {
        get
        {
            return AppBuilder.EnableSingleInstanceMode;
        }
    }

    /// <summary>
    /// Gets the <see cref="ChromiumEmbeddedEnvironment"/> instance used by the application.
    /// </summary>
    public ChromiumEmbeddedEnvironment ChromiumEmbeddedEnvironment
    {
        get
        {

            return AppBuilder.ChromiumEmbeddedEnvironment!;
        }
    }

    /// <summary>
    /// Creates a new <see cref="AppBuilder"/> for configuring and building a <see cref="WinFormiumApp"/> instance.
    /// </summary>
    /// <returns>The created <see cref="AppBuilder"/> instance.</returns>
    public static AppBuilder CreateBuilder()
    {
        if (_builder != null)
        {
            throw new InvalidOperationException("WinFormiumAppBuilder has already been created.");
        }

        _builder = new AppBuilder();

        return _builder;
    }

    /// <summary>
    /// Runs the main application process, initializing CEF, handling single instance logic, and launching the main form.
    /// </summary>
    public void Run()
    {
        var cmdArgs = Environment.GetCommandLineArgs();

        if (ProcessType == CefProcessId.Browser)
        {
        CefRedistributableCheck:
            var redistSettings = ChromiumEmbeddedEnvironment.CefRedistributableSettings;
            ChromiumEmbeddedEnvironment.ConfigureChromiumEmbeddedRedistributable?.Invoke(redistSettings);

            if (!redistSettings.IsCefRedistributableValid)
            {

                if (redistSettings.UseInternalRuntimeDownloader)
                {
                    var downloadForm = new DownloadForm(redistSettings);


                    if (downloadForm.ShowDialog() == DialogResult.OK)
                    {
                        redistSettings.AutoDetectCefRedistributable();
                        goto CefRedistributableCheck;
                    }
                }


                if (redistSettings.ThrowExceptionIfCefRedistributableIsNotValid)
                {
                    throw new DllNotFoundException("CEF distribution is not valid");
                }
                else
                {
                    Environment.Exit(-1);

                    return;
                }


            }

            if (SingleInstanceMode)
            {
                var thisProcess = Process.GetCurrentProcess();

                var processes = Process.GetProcessesByName(thisProcess.ProcessName);

                foreach (var process in processes)
                {
                    if (process.Id != thisProcess.Id && process.HandleCount > 0 && process.MainWindowHandle != 0)
                    {

                        var handler = AppBuilder.InternalSettings.Exists(nameof(AppBuilder.UseSingleRunningInstance)) ? AppBuilder.InternalSettings.GetValue<HandleInstanceIsRunningDelegate>(nameof(AppBuilder.UseSingleRunningInstance)) : null;


                        var args = new InstanceIsRunningEventArgs(process.Id, process.MainWindowHandle);

                        if (handler != null)
                        {
                            handler.Invoke(args);
                        }

                        if (!args.HasWindow) return;

                        if (args.ActivateMainWindow)
                        {
                            var targetHWND = new HWND(process.MainWindowHandle);

                            ShowWindowAsync(targetHWND, SHOW_WINDOW_CMD.SW_SHOWNOACTIVATE);

                            SetForegroundWindow(targetHWND);
                        }

                        return;
                    }
                }
            }

            if (AppBuilder.CacheShouldCleaned)
            {
                try
                {
                    var dirInfo = new DirectoryInfo(ChromiumEmbeddedEnvironment.CacheDiretory);
                    if (dirInfo.Exists)
                    {
                        dirInfo.Delete(true);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Error deleting cache directory: {ex.Message}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting cache directory: {ex.Message}");
                }
            }

            AppBuilder.AppStartup?.OnApplicationLaunched(cmdArgs);
        }

        var cefArgs = new CefMainArgs(cmdArgs);

        //if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 15063))
        //{
        //    SetProcessDpiAwarenessContext((DPI_AWARENESS_CONTEXT)(-4));
        //}
        //else if (OperatingSystem.IsWindowsVersionAtLeast(8, 1))
        //{
        //    SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE);
        //}
        //else
        //{
        //    SetProcessDPIAware();
        //}

        Application.CurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentUICulture = Culture;

        var browserApp = new BrowserApp(this);

        if (!LoadCefRuntime(cefArgs, browserApp, out var exitCode))
        {

            Environment.Exit(exitCode);

            return;
        }

        var env = ChromiumEmbeddedEnvironment;

        var settings = env.CreateDefaultSettings();
        settings.Locale = Culture.Name;

        settings.RootCachePath = env.AppDataDirectory;
        settings.UserDataPath = env.UserDataDirectory;
        settings.CachePath = SingleInstanceMode ? env.CacheDiretory : string.Empty;


        env.ConfigureDefaultChromiumSettings?.Invoke(settings);

        if (!ConfigureSubprocess(env, settings))
        {
            Environment.Exit(-2);
            return;
        }

        settings.LocalesDirPath = env.LocaleDirPath;
        settings.ResourcesDirPath = env.ResourceDirPath;

        settings.MultiThreadedMessageLoop = true;
        settings.ExternalMessagePump = false;



        CefRuntime.Initialize(cefArgs, settings, browserApp, IntPtr.Zero);
        CefRuntime.EnableHighDpiSupport();


        foreach (var factor in AppBuilder.ResourceSchemeHandlerFactories)
        {
            var factory = factor.Invoke();
            if (factory is not null)
            {
                CefRuntime.RegisterSchemeHandlerFactory(factory.Scheme, factory.DomainName, factory);
            }
        }

        try
        {
            var startupAction = AppBuilder.AppStartup?.OnApplicationStartup(new StartupSettings());

            if (startupAction == null)
            {
                Environment.Exit(0);
                return;
            }

            RunningApplicationContext = new StartupApplicationContext();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                AppBuilder.AppStartup?.OnApplicationTerminated();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                AppBuilder.AppStartup?.OnApplicationException(e.ExceptionObject as Exception);
            };

            startupAction.ConfigureAppContext(RunningApplicationContext);

            //if (RunningApplicationContext.MainForm is not null)
            //{
            //    var mainForm = RunningApplicationContext.MainForm;

            //    mainForm.FormClosed += (_, _) =>
            //    {
            //        for (int i = Application.OpenForms.Count - 1; i >= 0; --i)
            //        {
            //            var form = Application.OpenForms[i];
            //            if (form is null || form.IsDisposed) continue;

            //            if (form == mainForm) continue;

            //            form.Close();

            //        }
            //        CefRuntime.QuitMessageLoop();
            //    };

            //    mainForm.Show();
            //}

            //CefRuntime.RunMessageLoop();

            Application.Run(RunningApplicationContext);

        }
        finally
        {
            _pipeServer?.Dispose();

            Shutdown();
        }
    }

    /// <summary>
    /// Runs the application as a CEF subprocess.
    /// </summary>
    public void RunAsSubprocess()
    {
        var args = Environment.GetCommandLineArgs();
        var cefArgs = new CefMainArgs(args);

        Application.CurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentCulture = Culture;
        CultureInfo.DefaultThreadCurrentUICulture = Culture;

        var browserApp = new BrowserApp(this);


        LoadCefRuntime(cefArgs, browserApp, out var exitCode);

        Environment.Exit(exitCode);

    }

    /// <summary>
    /// Shuts down the CEF runtime and performs cleanup.
    /// </summary>
    public void Shutdown()
    {
        CefRuntime.Shutdown();
    }

    /// <summary>
    /// The name of the process pipe used for inter-process communication between the browser and renderer processes.
    /// </summary>
    List<Action>? WindowBindingCreationScripts;

    /// <summary>
    /// Invokes the window binding creation scripts after WebKit has been initialized in the renderer process.
    /// </summary>
    internal void OnWebKitInitialized()
    {
        if (ProcessType == CefProcessId.Renderer)
        {
            // Execute the scripts to create window bindings in the renderer process.
            if (WindowBindingCreationScripts != null)
            {
                foreach (var script in WindowBindingCreationScripts)
                {
                    script.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// The constant name of the WinFormium application.
    /// </summary>
    internal const string WINFROMIUM_NAME = "WinFormium";

    /// <summary>
    /// Initializes a new instance of the <see cref="WinFormiumApp"/> class.
    /// </summary>
    /// <param name="appBuilder">The <see cref="AppBuilder"/> to use for configuration, or null to use the static builder.</param>
    internal WinFormiumApp(AppBuilder? appBuilder = null)
    {
        var builder = appBuilder ?? _builder;

        if (builder == null) throw new InvalidOperationException("WinFormiumAppBuilder has not been created.");

        AppBuilder = builder;

        if (ProcessType == CefProcessId.Renderer)
        {
            var args = Environment.GetCommandLineArgs();

            var processIdArg = args.FirstOrDefault(x => x.StartsWith("--host-process-id"));


            BrowserProcessId = processIdArg == null ? 0 : int.Parse(Regex.Replace(processIdArg, "--host-process-id=", string.Empty));

            if (BrowserProcessId == 0)
            {
                throw new ApplicationException("Browser process id is not found.");
            }

            WindowBindingCreationScripts = AppBuilder.WebKitInitializedActions;

            MainProcessMonitor.StartMonitoring(BrowserProcessId);
        }
        else
        {
            BrowserProcessId = Process.GetCurrentProcess().Id;

            _pipeServer = new PipeServer(ProcessPipeName);
            _pipeServer.MessageReceived += RenderProcessMessageReceived;

        }





    }

    /// <summary>
    /// Gets the collection of renderer process message handlers.
    /// </summary>
    internal static ConcurrentDictionary<string, Func<string?, string?>> RendererProcessMessageHandlers { get; } = new();

    /// <summary>
    /// Gets the <see cref="AppBuilder"/> used to configure this application.
    /// </summary>
    internal AppBuilder AppBuilder { get; }

    /// <summary>
    /// Gets the name of the process pipe for inter-process communication.
    /// </summary>
    internal string ProcessPipeName
    {
        get
        {
            return $"WinFormium_ServerPipe_{BrowserProcessId}";
        }
    }

    /// <summary>
    /// Gets the running application context, if available.
    /// </summary>
    internal StartupApplicationContext? RunningApplicationContext { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the system is in dark mode.
    /// </summary>
    internal bool IsDarkMode => GetSystemColorMode() == SystemColorMode.Dark;

    /// <summary>
    /// Gets the current system color mode (light or dark).
    /// </summary>
    /// <returns>The <see cref="SystemColorMode"/> value.</returns>
    internal SystemColorMode GetSystemColorMode()
    {

        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        static extern bool IsDarkMode();

        if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18362))
        {
            try
            {
                if (AppBuilder.SystemColorMode == SystemColorMode.Auto)
                {
                    return IsDarkMode() ? SystemColorMode.Dark : SystemColorMode.Light;
                }
                else
                {
                    return AppBuilder.SystemColorMode == SystemColorMode.Dark ? SystemColorMode.Dark : SystemColorMode.Light;
                }

            }
            catch
            {

            }
        }

        return SystemColorMode.Light;
    }

    /// <summary>
    /// Registers a message handler for the renderer process.
    /// </summary>
    /// <param name="name">The name of the message.</param>
    /// <param name="handler">The handler function to register.</param>
    internal void RegisterRendererProcessMessageHandler(string name, Func<string?, string?> handler)
    {
        RendererProcessMessageHandlers[name] = handler;
    }

    /// <summary>
    /// Unregisters a message handler for the renderer process.
    /// </summary>
    /// <param name="name">The name of the message to unregister.</param>
    internal void UnregisterRendererProcessMessageHandler(string name)
    {
        RendererProcessMessageHandlers.TryRemove(name, out _);
    }

    /// <summary>
    /// Sends a message to the browser process from the renderer process.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>The response from the browser process, or null if not applicable.</returns>
    internal string? SendMessageToBrowserProcess(RenderProcessMessage message)
    {

        if (ProcessType != CefProcessId.Renderer) return null;

        var client = new PipeClient(ProcessPipeName);

        var result = client.SendMessage(message.ToJson());

        return result;

    }

    /// <summary>
    /// Sends a message to the browser process and deserializes the response to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="message">The message to send.</param>
    /// <param name="jsonTypeInfo">The JSON type information for deserialization.</param>
    /// <returns>The deserialized response, or default if not available.</returns>
    internal T? SendMessageToBrowserProcess<T>(RenderProcessMessage message, JsonTypeInfo<T> jsonTypeInfo)
    {
        var result = SendMessageToBrowserProcess(message);

        if (string.IsNullOrEmpty(result))
        {
            return default;
        }


        try
        {
            return JsonSerializer.Deserialize(result, jsonTypeInfo);
        }
        catch
        {

        }

        return default;
    }

    private static AppBuilder? _builder;
    private static WinFormiumApp? _current;
    private PipeServer? _pipeServer;

    /// <summary>
    /// Handles messages received from the render process via the pipe server.
    /// </summary>
    /// <param name="data">The message data.</param>
    /// <param name="success">Indicates whether the message was received successfully.</param>
    /// <param name="exception">The exception, if any, that occurred during message processing.</param>
    /// <returns>The response to send back to the render process.</returns>
    private string RenderProcessMessageReceived(string data, bool success, Exception? exception)
    {
        if (success)
        {
            try
            {
                var message = RenderProcessMessage.FromJson(data);

                if (message == null) return string.Empty;

                if (RendererProcessMessageHandlers.TryGetValue(message.Name, out var handlers))
                {
                    return handlers.Invoke(message.Data) ?? string.Empty;
                }
            }
            catch
            {

            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Loads the CEF runtime and executes the process.
    /// </summary>
    /// <param name="args">The CEF main arguments.</param>
    /// <param name="app">The CEF application instance.</param>
    /// <param name="exitCode">The exit code returned by the process.</param>
    /// <returns>True if the runtime was loaded successfully; otherwise, false.</returns>
    private bool LoadCefRuntime(CefMainArgs args, CefApp app, [Optional] out int exitCode)
    {

        try
        {

            CefRuntime.Load(ChromiumEmbeddedEnvironment.LibCefDirPath);


            // Render process start and block here.
            exitCode = CefRuntime.ExecuteProcess(args, app, IntPtr.Zero);

            //TODO:本地化错误信息
            if (exitCode != -1)
            {

                Debug.WriteLine($"ExecuteProcess() expected to return -1 but returned {exitCode}");

                return false;
            }


            foreach (var arg in Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("--type="))
                {

                    exitCode = -2;

                    Debug.WriteLine($"ExecuteProcess() expected to return -1 but returned {exitCode}");

                    return false;
                }
            }

            Debug.WriteLine($"ExecuteProcess() returns {exitCode} as expected.");

        }
        catch (CefVersionMismatchException ex)
        {

            CefRuntime.Shutdown();

            WinFormMessageBox.Show($"CefVersionMismatchException:{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            exitCode = -2;

            return false;

        }
        catch (DllNotFoundException ex)
        {


            WinFormMessageBox.Show($"DllNotFoundException:{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            exitCode = -2;

            return false;
        }
        catch (Exception ex)
        {

            Debugger.Launch();

            Debug.WriteLine(ex);
            throw;
        }


        exitCode = 0;

        return true;
    }

    /// <summary>
    /// Configures the subprocess application settings for CEF.
    /// </summary>
    /// <param name="env">The Chromium embedded environment.</param>
    /// <param name="settings">The CEF settings to configure.</param>
    /// <returns>True if the configuration was successful; otherwise, false.</returns>
    private bool ConfigureSubprocess(ChromiumEmbeddedEnvironment env, CefSettings settings)
    {
        if (env.ConfigureSubprocessAppSettings == null) return true;

        var subprocessSettings = new SubprocessAppSettings(Architecture);

        env.ConfigureSubprocessAppSettings(subprocessSettings);

        if (!subprocessSettings.SubprocessPathIsSet) return true;

        if (File.Exists(subprocessSettings.SubprocessFilePath))
        {
            var subprocessFile = new FileInfo(subprocessSettings.SubprocessFilePath);

            settings.BrowserSubprocessPath = subprocessFile.FullName;

            return true;

        }
        else if (subprocessSettings.ThrowExceptionIfSubprocessIsNotExists)
        {
            throw new FileNotFoundException($"Subprocess file not found: {subprocessSettings.SubprocessFilePath}");
        }
        else
        {
            return false;
        }
    }
}


//class CefMessagePumpMessageFilter : IMessageFilter
//{
//    public bool PreFilterMessage(ref Message m)
//    {
//        CefRuntime.DoMessageLoopWork();
//        return false;
//    }
//}