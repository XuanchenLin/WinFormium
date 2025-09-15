// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace WinFormium;

/// <summary>
/// Delegate for configuring browser settings before browser creation.
/// </summary>
internal delegate void OnConfigureBrowserSettingsDelegate(CefBrowserSettings browserSettings);

/// <summary>
/// Core class for managing the embedded Chromium browser in WinFormium.
/// Handles browser creation, message interception, DevTools integration, DPI scaling, and system color mode.
/// </summary>
internal partial class WebViewCore
{
    #region DevToolClient

    /// <summary>
    /// Represents a client for the DevTools window, handling its creation, resizing, and browser life span events.
    /// </summary>
    private class DevToolClient : CefClient, ILifeSpanHandler
    {
        /// <summary>
        /// Gets the <see cref="CefBrowser"/> instance for the DevTools window.
        /// </summary>
        public CefBrowser? Browser { get; private set; }

        /// <summary>
        /// Gets the <see cref="CefBrowserHost"/> associated with the DevTools window.
        /// </summary>
        public CefBrowserHost BrowserHost { get; }

        /// <summary>
        /// Gets the host form that contains the DevTools window.
        /// </summary>
        private Form DevToolsForm { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DevToolClient"/> class.
        /// </summary>
        /// <param name="webview">The parent <see cref="WebViewCore"/> instance.</param>
        public DevToolClient(WebViewCore webview)
        {
            var browserHost = webview.BrowserHost;

            if (browserHost == null)
            {
                throw new NullReferenceException(nameof(browserHost));
            }

            var scrWidth = GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN);

            var scrHeight = GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSCREEN);

            var width = (int)(scrWidth * 0.7f);
            var height = (int)(scrHeight * 0.7f);

            if (!webview.ContainerHWND.IsNull)
            {
                GetWindowRect(webview.ContainerHWND, out var rect);

                width = Math.Min(width, rect.Width);
                height = Math.Min(height, rect.Height);
            }

            width = Math.Max(width, 960);
            height = Math.Max(height, 680);

            DevToolsForm = new Form()
            {
                Text = "DevTools",
                Width = width,
                Height = height,
                Icon = Resources.Icons.DevTools,
                MinimumSize = new Size(960, 640),
                AutoScaleMode = AutoScaleMode.Dpi
            };

            DevToolsForm.HandleCreated += WindowHandleCreated;
            BrowserHost = browserHost;
        }

        /// <summary>
        /// Closes the DevTools window.
        /// </summary>
        public void Close()
        {
            DevToolsForm.Close();
        }

        /// <summary>
        /// Shows the DevTools window.
        /// </summary>
        public void Show(IWin32Window? window = null)
        {
            if (window is null)
            {
                DevToolsForm.Show();
            }
            else
            {
                DevToolsForm.Show(window);
            }
        }

        /// <inheritdoc/>
        bool ILifeSpanHandler.DoClose(CefBrowser browser)
        {
            return false;
        }

        /// <inheritdoc/>
        void ILifeSpanHandler.OnAfterCreated(CefBrowser browser)
        {
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            Browser = browser;

            DevToolsForm.Resize += (_, _) =>
            {
                var handle = browser.GetHost()?.GetWindowHandle() ?? 0;

                if (handle == 0) return;

                GetClientRect((HWND)DevToolsForm.Handle, out var rect);

                SetWindowPos((HWND)handle, HWND.Null, 0, 0, rect.Width, rect.Height, SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER);
            };

            DevToolsForm.ResizeBegin += (_, _) => browser.GetHost()?.NotifyMoveOrResizeStarted();
            DevToolsForm.ResizeEnd += (_, _) => browser.GetHost()?.WasResized();

            DevToolsForm.Move += (_, _) =>
            {
                browser.GetHost()?.NotifyMoveOrResizeStarted();
                //browser.GetHost().SendCaptureLostEvent();
            };

            DevToolsForm.DpiChanged += (_, _) =>
            {
                browser.GetHost()?.NotifyScreenInfoChanged();
                browser.GetHost()?.WasResized();
            };

            //DevToolsForm.Activated += (_, _) => browser.GetHost()?.SetFocus(true);
            //DevToolsForm.Deactivate += (_, _) => browser.GetHost()?.SetFocus(false);
        }

        /// <inheritdoc/>
        void ILifeSpanHandler.OnBeforeClose(CefBrowser browser)
        {
        }

        /// <inheritdoc/>
        bool ILifeSpanHandler.OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
        {
            return false;
        }

        /// <inheritdoc/>
        protected override CefLifeSpanHandler? GetLifeSpanHandler()
        {
            return new WebViewLifeSpanHandler(this);
        }

        /// <summary>
        /// Handles the creation of the DevTools window handle and initializes the DevTools browser.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void WindowHandleCreated(object? sender, EventArgs e)
        {
            var windowInfo = CefWindowInfo.Create();

            //windowInfo.StyleEx |= CefGlue.Platform.Windows.WindowStyleEx.WS_EX_NOACTIVATE;

            //windowInfo.Style |= CefGlue.Platform.Windows.WindowStyle.WS_CHILD | CefGlue.Platform.Windows.WindowStyle.WS_MAXIMIZE;

            windowInfo.SetAsChild(DevToolsForm.Handle, new CefRectangle(0, 0, DevToolsForm.ClientSize.Width, DevToolsForm.ClientSize.Height));

            BrowserHost.ShowDevTools(windowInfo, this, new CefBrowserSettings(), new CefPoint(0, 0));
        }
    }

    #endregion DevToolClient

    #region BrowserMessageInterceptor

    /// <summary>
    /// Intercepts and forwards Windows messages for the browser window, and provides utilities for finding Chrome's render widget host window.
    /// </summary>
    private class BrowserMessageInterceptor : NativeWindow, IDisposable
    {
        /// <summary>
        /// The delegate used to forward window messages.
        /// </summary>
        private WndProcDelegate _forwardAction;

        /// <summary>
        /// Gets the HWND of the browser window being intercepted.
        /// </summary>
        public HWND BrowserWindowHWND { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserMessageInterceptor"/> class.
        /// </summary>
        /// <param name="browserWindowHWND">The HWND of the browser window to intercept.</param>
        /// <param name="forwardAction">The delegate to forward window messages.</param>
        internal BrowserMessageInterceptor(HWND browserWindowHWND, WndProcDelegate forwardAction)
        {
            BrowserWindowHWND = browserWindowHWND;
            AssignHandle(browserWindowHWND);

            _forwardAction = forwardAction;
        }

        /// <summary>
        /// Asynchronously sets up the message interceptor by finding the Chrome_RenderWidgetHostHWND child window.
        /// </summary>
        /// <param name="browserHwnd">The HWND of the browser window.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the found HWND, or HWND.Null if not found.</returns>
        public static Task<HWND> Setup(HWND browserHwnd)
        {
            return Task.Run<HWND>(() =>
            {
                try
                {
                    var retryRemains = 10;

                    if (browserHwnd.IsNull) return HWND.Null;

                    while (retryRemains > 0)
                    {
                        if (ChromeRenderWidgetHostHwndFinder.TryFindRenderWidgetHostHwnd(browserHwnd, out var renderWidgetHostHwnd))
                        {
                            return renderWidgetHostHwnd;
                        }

                        retryRemains--;
                        Thread.Sleep(100);
                    }

                    // If we reach here, we couldn't find the render widget host hwnd
                    return HWND.Null;
                }
                catch
                {
                    return HWND.Null;
                }
            });
        }

        /// <summary>
        /// Releases the handle associated with this window.
        /// </summary>
        public void Dispose()
        {
            ReleaseHandle();
        }

        /// <inheritdoc/>
        protected override void WndProc(ref Message m)
        {
            if (!_forwardAction.Invoke(ref m))
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Provides methods to find the Chrome_RenderWidgetHostHWND child window.
        /// </summary>
        private class ChromeRenderWidgetHostHwndFinder
        {
            /// <summary>
            /// The class name of the Chrome render widget host window.
            /// </summary>
            private const string RENDER_WIDGET_HOST_HWND_CLASS_NAME = "Chrome_RenderWidgetHostHWND";

            /// <summary>
            /// Attempts to find the Chrome_RenderWidgetHostHWND child window for the specified parent window.
            /// </summary>
            /// <param name="hwnd">The parent window handle.</param>
            /// <param name="renderWidgetHostHwnd">When this method returns, contains the found render widget host HWND, or HWND.Null if not found.</param>
            /// <returns>True if the render widget host HWND was found; otherwise, false.</returns>
            internal static bool TryFindRenderWidgetHostHwnd(HWND hwnd, out HWND renderWidgetHostHwnd)
            {
                var target = new RenderWidgetHostHwndWrapper();
                var gcHandle = GCHandle.Alloc(target, GCHandleType.Pinned);

                try
                {
                    EnumChildWindows(hwnd, EnumWindowFunc, GCHandle.ToIntPtr(gcHandle));
                }
                finally
                {
                    gcHandle.Free();
                }

                renderWidgetHostHwnd = target.DescendantFound;
                return renderWidgetHostHwnd != HWND.Null;
            }

            /// <summary>
            /// Callback function for enumerating child windows to find the render widget host window.
            /// </summary>
            /// <param name="hwnd">The handle of the window being enumerated.</param>
            /// <param name="lParam">A pointer to a GCHandle for the target wrapper.</param>
            /// <returns>True to continue enumeration, false to stop.</returns>
            private static BOOL EnumWindowFunc(HWND hwnd, LPARAM lParam)
            {
                var buffer = new Span<char>(new char[256]);
                GetClassName(hwnd, buffer);
                var className = new string(buffer.TrimEnd('\0').ToArray());

                if (className == RENDER_WIDGET_HOST_HWND_CLASS_NAME)
                {
                    var gcHandle = GCHandle.FromIntPtr(lParam);
                    var target = (RenderWidgetHostHwndWrapper)gcHandle.Target!;
                    target.DescendantFound = hwnd;
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Wrapper class to hold the HWND of a found descendant window.
        /// </summary>
        private class RenderWidgetHostHwndWrapper
        {
            /// <summary>
            /// Gets or sets the HWND of the found descendant window.
            /// </summary>
            public HWND DescendantFound { get; set; }
        }
    }

    #endregion BrowserMessageInterceptor

    /// <summary>
    /// The handle of the container control, used for browser creation and resizing.
    /// </summary>
    private nint _containerHandle = 0;

    /// <summary>
    /// Stores a deferred URL to be loaded when the browser is ready.
    /// </summary>
    private string? _defferedUrl = null;

    // Host control
    /// <summary>
    /// The host control for the browser instance.
    /// </summary>
    private Control _hostControl;

    /// <summary>
    /// Temporary container control used during handle recreation.
    /// </summary>
    private Control? _temporaryContainerControl;

    /// <summary>
    /// Occurs when the browser has been created and initialized.
    /// </summary>
    public event EventHandler? BrowserCreated;

    /// <summary>
    /// Gets the client interface for handling browser events.
    /// </summary>
    public IWebViewClient BrowserClient { get; }

    /// <summary>
    /// Gets or sets the delegate for handling browser window messages.
    /// </summary>
    public WndProcDelegate? BrowserWindowProc { get; set; } = null;

    /// <summary>
    /// Gets the process communication bridge for inter-process communication.
    /// </summary>
    public ProcessCommunicationBridge? CommunicationBridge { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether offscreen rendering is enabled.
    /// </summary>
    public bool EnableOffscreenRendering { get; init; }

    /// <summary>
    /// Gets the JavaScript engine for executing scripts in the browser.
    /// </summary>
    public JavaScriptEngine? JsEngine { get; private set; }

    /// <summary>
    /// Gets the CefRequestContext associated with the browser.
    /// </summary>
    public CefRequestContext? RequestContext { get; }

    /// <summary>
    /// Gets the scale factor for DPI scaling.
    /// </summary>
    public float ScaleFactor => (ContainerForm.DeviceDpi / 96f);

    /// <summary>
    /// Gets or sets a value indicating whether to simplify the context menu.
    /// </summary>
    public bool SimplifyContextMenu { get; set; } = false;

    /// <summary>
    /// Gets or sets the URL currently loaded or to be loaded in the browser.
    /// </summary>
    public string Url
    {
        get => Browser?.GetMainFrame()?.Url ?? _defferedUrl ?? "about:blank";
        set
        {
            var url = $"{value}".Trim();

            if (Browser != null)
            {
                ActionTask.Run(() => Browser.GetMainFrame().LoadUrl(url));
            }
            else
            {
                _defferedUrl = url;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use a ContextMenuStrip for the browser.
    /// </summary>
    public bool UseContextMenuStrip { get; set; } = false;

    /// <summary>
    /// Gets the window handle of the browser.
    /// </summary>
    internal nint BrowserHandle => BrowserHost?.GetWindowHandle() ?? 0;

    /// <summary>
    /// Gets the HWND of the browser window.
    /// </summary>
    internal HWND BrowserHWND => (HWND)BrowserHandle;

    /// <summary>
    /// Gets the handle of the Chrome Render Widget Host window.
    /// </summary>
    internal nint ChromeRenderWidgetHostHandle => MessageInterceptor?.Handle ?? 0;

    /// <summary>
    /// Gets the HWND of the Chrome Render Widget Host window.
    /// </summary>
    internal HWND ChromeRenderWidgetHostHWND => new(ChromeRenderWidgetHostHandle);

    internal HWND ContainerFormHWND { get; private set; }

    /// <summary>
    /// Gets the HWND of the container control.
    /// </summary>
    internal HWND ContainerHWND => new(ContainerHandle);

    /// <summary>
    /// Gets a value indicating whether offscreen rendering is currently used.
    /// </summary>
    internal bool IsOsrRendering => (BrowserHandle == ContainerHandle) || (Initialized && BrowserHWND.IsNull);

    /// <summary>
    /// Gets or sets the delegate for configuring browser settings before creation.
    /// </summary>
    internal OnConfigureBrowserSettingsDelegate? OnConfigureBrowserSettings { get; set; }

    /// <summary>
    /// Gets the underlying CefBrowser instance.
    /// </summary>
    protected internal CefBrowser? Browser { get; private set; }

    // Browser
    /// <summary>
    /// Gets the CefBrowserHost associated with the browser.
    /// </summary>
    protected internal CefBrowserHost? BrowserHost => Browser?.GetHost();

    /// <summary>
    /// Gets the container control that hosts the browser.
    /// </summary>
    protected internal Control Container => _hostControl;

    /// <summary>
    /// Gets the container form that hosts the browser.
    /// </summary>
    protected internal Form ContainerForm => Container is Form ? (Form)Container : Container.TopLevelControl as Form ?? throw new InvalidOperationException("Host control is not a form or top level control");
    /// <summary>
    /// Gets the handle of the container control.
    /// </summary>
    protected internal nint ContainerHandle => _containerHandle;
    // Browser Widget

    /// <summary>
    /// Gets a value indicating whether the browser has been initialized.
    /// </summary>
    protected internal bool Initialized { get; private set; } = false;

    /// <summary>
    /// Gets or sets the message interceptor for the browser widget.
    /// </summary>
    private BrowserMessageInterceptor? MessageInterceptor { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="WebViewCore"/> class.
    /// </summary>
    /// <param name="control">The host control for the browser.</param>
    /// <param name="client">The client interface for browser events.</param>
    public WebViewCore(Control control, IWebViewClient client)
    {
        ArgumentNullException.ThrowIfNull(control, nameof(control));

        _hostControl = control;

        BrowserClient = client;

        _internalResourceRequestHandler = new WebResourceRequestHandler(this);

        Container.HandleCreated += ContainerHandleCreated;
        Container.HandleDestroyed += ContainerHandleDestroyed;
    }
    /// <summary>
    /// Handles window messages for the host control.
    /// </summary>
    /// <param name="m">The window message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    public bool HandleHostWndProc(ref Message m)
    {
        var msg = (uint)m.Msg;

        if (UseContextMenuStrip && (msg == WM_PARENTNOTIFY || msg == WM_SYSCOMMAND))
        {
            CloseContextMenu();
        }

        if (msg == WM_MOVE || msg == WM_MOVING)
        {
            BrowserHost?.NotifyMoveOrResizeStarted();
        }

        if (msg == WM_SETTINGCHANGE && m.LParam != 0)
        {
            OnWmSettingChangeWithImmersiveColorSet(m.LParam);
        }

        return false;
    }

    /// <summary>
    /// Hides the browser window.
    /// </summary>
    public void Hide()
    {
        BrowserHost?.WasHidden(true);

        if (!Initialized || BrowserHost == null || IsOsrRendering) return;

        SetWindowPos(BrowserHWND, HWND.Null, 0, 0, 0, 0,
                 SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
    }

    /// <summary>
    /// Hides the DevTools window for the browser.
    /// </summary>
    public void HideDevTools()
    {
        if (BrowserHost is null || Container is null) return;
        if (Container.InvokeRequired)
        {
            Container.Invoke(HideDevTools);
            return;
        }

        BrowserHost.CloseDevTools();
    }

    /// <summary>
    /// Invokes the specified action on the UI thread if required.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    public void InvokeIfRequired(Action action)
    {
        if (Container.InvokeRequired)
            Container.Invoke(action);
        else
            action();
    }

    /// <summary>
    /// Invokes the specified delegate on the UI thread if required.
    /// </summary>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="args">The arguments to pass to the delegate.</param>
    /// <returns>The result of the delegate invocation.</returns>
    public object? InvokeIfRequired(Delegate method, params object[] args)
    {
        if (Container.InvokeRequired)
            return Container.Invoke(method, args);
        else
            return method.DynamicInvoke(args);
    }

    /// <summary>
    /// Invokes the specified delegate on the UI thread if required and returns a result.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="args">The arguments to pass to the delegate.</param>
    /// <returns>The result of the delegate invocation.</returns>
    public T? InvokeIfRequired<T>(Delegate method, params object[] args)
    {
        if (Container.InvokeRequired)
            return (T?)Container.Invoke(method, args);
        else
            return (T?)method.DynamicInvoke(args);
    }

    /// <summary>
    /// Invokes the specified function on the UI thread if required and returns a result.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="func">The function to invoke.</param>
    /// <returns>The result of the function invocation.</returns>
    public T? InvokeIfRequired<T>(Func<T> func)
    {
        if (Container.InvokeRequired)
            return Container.Invoke(func);
        else
            return func();
    }

    /// <summary>
    /// Resizes the browser window to fit the container.
    /// </summary>
    public void Resize()
    {
        if (!Initialized || BrowserHost == null) return;

        BrowserHost?.WasResized();

        if (ContainerFormHWND.IsNull)
        {
            return;
        }

        if (IsIconic(ContainerFormHWND))
        {
            Hide();
            return;
        }

        Show();

        if (IsOsrRendering) return;

        GetClientRect(ContainerFormHWND, out var rect);

        SetWindowPos(BrowserHWND, HWND.Null, 0, 0, rect.Width, rect.Height, SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
    }

    /// <summary>
    /// Shows the browser window.
    /// </summary>
    public void Show()
    {
        BrowserHost?.WasHidden(false);

        if (!Initialized || BrowserHost == null || IsOsrRendering) return;

        if (!IsWindowVisible(BrowserHWND))
        {
            ShowWindow(BrowserHWND, SHOW_WINDOW_CMD.SW_SHOW);
        }
    }

    /// <summary>
    /// Shows the DevTools window for the browser.
    /// </summary>
    public void ShowDevTools()
    {
        if (BrowserHost is null || Container is null) return;

        if (Container.InvokeRequired)
        {
            Container.Invoke(ShowDevTools);
            return;
        }

        var devTools = new DevToolClient(this);

        devTools.Show();
    }

    /// <summary>
    /// Handles the creation of the container control's handle.
    /// </summary>
    private void ContainerHandleCreated(object? sender, EventArgs e)
    {
        _containerHandle = Container.Handle;

        ContainerFormHWND = (HWND)ContainerForm.Handle;

        if (Container.RecreatingHandle && !IsOsrRendering)
        {
            if (_temporaryContainerControl == null) throw new NullReferenceException("Temporary container control is null.");

            SetParent(BrowserHWND, ContainerHWND);

            _temporaryContainerControl.Dispose();
            _temporaryContainerControl = null;

            Resize();
        }
        else
        {
            CreateBrowser(EnableOffscreenRendering);
        }

        HandleSystemColorMode();
    }

    /// <summary>
    /// Handles the destruction of the container control's handle.
    /// </summary>
    private void ContainerHandleDestroyed(object? sender, EventArgs e)
    {
        if (Container.RecreatingHandle && !IsOsrRendering)
        {
            _temporaryContainerControl = new Control();
            _temporaryContainerControl.CreateControl();
            SetParent(BrowserHWND, (HWND)_temporaryContainerControl.Handle);
        }
    }

    /// <summary>
    /// Creates the browser instance with the specified rendering mode.
    /// </summary>
    /// <param name="enableOsr">True to enable offscreen rendering; otherwise, false.</param>
    private void CreateBrowser(bool enableOsr = false)
    {
        var windowInfo = CefWindowInfo.Create();

        windowInfo.StyleEx |= WinFormium.Browser.CefGlue.Platform.Windows.WindowStyleEx.WS_EX_NOACTIVATE;

        if (enableOsr)
        {
            windowInfo.SetAsWindowless(ContainerHandle, true);
        }
        else
        {
            windowInfo.SetAsChild(ContainerHandle, new CefRectangle(0, 0, 0, 0));
        }

        var browserSettings = WinFormiumApp.Current.ChromiumEmbeddedEnvironment.CreateDefaultBrowserSettings();

        WinFormiumApp.Current?.ChromiumEmbeddedEnvironment?.ConfigureDefaultBrowserSettings?.Invoke(browserSettings);

        OnConfigureBrowserSettings?.Invoke(browserSettings);

        Container.BackColor = Color.FromArgb((int)(browserSettings.BackgroundColor.ToArgb() | 0xFF000000));

        using var extraInfo = CefDictionaryValue.Create();

        extraInfo.SetInt("BrowserProcessId", WinFormiumApp.Current!.BrowserProcessId);

        CefBrowserHost.CreateBrowser(windowInfo, this, browserSettings, Url, extraInfo, RequestContext!);

        _defferedUrl = null;
    }

    /// <summary>
    /// Applies the system color mode (dark/light) to the container form.
    /// </summary>
    private void HandleSystemColorMode()
    {
        BOOL mode = WinFormiumApp.Current.GetSystemColorMode() == SystemColorMode.Dark ? true : false;

        if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
        {
            unsafe
            {
                DwmSetWindowAttribute((HWND)ContainerForm.Handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, &mode, (uint)sizeof(BOOL));
            }
        }
    }

    /// <summary>
    /// Handles browser creation and attaches necessary event handlers.
    /// </summary>
    /// <param name="browser">The created CefBrowser instance.</param>
    private void OnBrowserCreatedCore(CefBrowser browser)
    {
        //Container.DpiChangedAfterParent += (_, _) =>
        //{
        //    BrowserHost?.NotifyScreenInfoChanged();

        //    Resize();
        //};

        ContainerForm.ResizeBegin += (_, _) => BrowserHost?.NotifyMoveOrResizeStarted();
        ContainerForm.ResizeEnd += (_, _) => BrowserHost?.WasResized();
        Container.Resize += (_, _) => Resize();
        ContainerForm.DpiChanged += (_, _) =>
        {
            BrowserHost?.NotifyScreenInfoChanged();
            BrowserHost?.WasResized();
        };


        ContainerForm.FormClosing += (_, e) =>
        {
            //if (IsOsrRendering) return;
            if (!IsClosing)
            {
                e.Cancel = true;
                Close();
                return;
            }
        };


        ContainerForm.VisibleChanged += (_, _) => BrowserHost?.WasHidden(!ContainerForm.Visible);

        //ContainerForm.GotFocus += (_, _) => BrowserHost?.SetFocus(true);
        //ContainerForm.LostFocus += (_, _) => BrowserHost?.SetFocus(false);

        BrowserCreated?.Invoke(this, EventArgs.Empty);
    }
    /// <inheritdoc/>
    private bool OnBrowserWndProcCore(ref Message m)
    {
        var msg = (uint)m.Msg;

        return (BrowserWindowProc?.Invoke(ref m) ?? false);
    }

    /// <summary>
    /// Handles the WM_SETTINGCHANGE message for immersive color set changes.
    /// </summary>
    /// <param name="lParam">The message parameter.</param>
    private void OnWmSettingChangeWithImmersiveColorSet(nint lParam)
    {
        const string IMMERSIVE_COLOR_SET = "ImmersiveColorSet";

        const int strlen = 255;

        var buffer = new byte[strlen];

        Marshal.Copy(lParam, buffer, 0, buffer.Length);

        var setting = Encoding.Unicode.GetString(buffer);

        setting = setting.Substring(0, setting.IndexOf('\0'));

        if (setting == IMMERSIVE_COLOR_SET)
        {
            HandleSystemColorMode();
        }
    }

    /// <summary>
    /// Asynchronously sets up the browser message interceptor for the browser window.
    /// </summary>
    private async void SetupBrowserMessageInterceptor()
    {
        if (IsOsrRendering)
        {
            return;
        }

        var handle = await BrowserMessageInterceptor.Setup(BrowserHWND);

        if (handle.IsNull) return;

        MessageInterceptor?.Dispose();

        MessageInterceptor = new BrowserMessageInterceptor(handle, OnBrowserWndProcCore);
    }
}