// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

public partial class Formium
{


    /// <summary>
    /// Occurs when a web message is received from the browser.
    /// </summary>
    public event EventHandler<WebMessageReceivedEventArgs>? WebMessageReceived;

    /// <summary>
    /// Gets or sets a value indicating whether drag-and-drop is allowed.
    /// </summary>
    public bool AllowDrop { get; set; } = false;

    /// <summary>
    /// Gets or sets the current URL of the browser.
    /// </summary>
    public string Url { get => WebView.Url; set => WebView.Url = value; }

    /// <summary>
    /// Executes the specified JavaScript code in the main frame.
    /// </summary>
    /// <param name="code">The JavaScript code to execute.</param>
    /// <param name="url">The URL of the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    public void ExecuteJavaScript(string code, string? url = null, int line = 0)
    {
        WebView.ExecuteJavaScript(code, url, line);
    }

    /// <summary>
    /// Executes the specified JavaScript code in the given frame.
    /// </summary>
    /// <param name="frame">The target frame.</param>
    /// <param name="code">The JavaScript code to execute.</param>
    /// <param name="url">The URL of the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    public void ExecuteJavaScript(CefFrame frame, string code, string? url = null, int line = 0)
    {
        WebView.ExecuteJavaScript(frame, code, url, line);
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the main frame and returns the result as a string.
    /// </summary>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL of the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as a string.</returns>
    public Task<string?> EvaluateJavaScriptAsync(string code, string? url = null, int line = 0)
    {
        return WebView.EvaluateJavaScriptAsync(code, url, line);
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the specified frame and returns the result as a string.
    /// </summary>
    /// <param name="frame">The target frame.</param>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL of the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as a string.</returns>
    public Task<string?> EvaluateJavaScriptAsync(CefFrame frame, string code, string? url = null, int line = 0)
    {
        return WebView.EvaluateJavaScriptAsync(frame, code, url, line);
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the main frame and returns a detailed result.
    /// </summary>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL of the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as <see cref="JavaScriptExecuteScriptResult"/>.</returns>
    public Task<JavaScriptExecuteScriptResult> EvaluateJavaScriptWithResultAsync(string code, string? url = null, int line = 0)
    {
        return WebView.EvaluateJavaScriptWithResultAsync(code, url, line);
    }

    /// <summary>
    /// Asynchronously evaluates JavaScript code in the specified frame and returns a detailed result.
    /// </summary>
    /// <param name="frame">The target frame.</param>
    /// <param name="code">The JavaScript code to evaluate.</param>
    /// <param name="url">The URL of the script (optional).</param>
    /// <param name="line">The base line number for error reporting (optional).</param>
    /// <returns>A task representing the asynchronous operation, with the result as <see cref="JavaScriptExecuteScriptResult"/>.</returns>
    public Task<JavaScriptExecuteScriptResult> EvaluateJavaScriptWithResultAsync(CefFrame frame, string code, string? url = null, int line = 0)
    {
        return WebView.EvaluateJavaScriptWithResultAsync(frame, code, url, line);
    }

    /// <summary>
    /// Adds a script to be executed when a new document is created.
    /// </summary>
    /// <param name="script">The JavaScript code to execute.</param>
    /// <returns>An identifier for the added script.</returns>
    public int AddScriptToExecuteOnDocumentCreated(string script)
    {
        return WebView.AddScriptToExecuteOnDocumentCreated(script);
    }

    /// <summary>
    /// Removes a script previously added to execute on document creation.
    /// </summary>
    /// <param name="identifier">The identifier of the script to remove.</param>
    public void RemoveScriptToExecuteOnDocumentCreated(int identifier)
    {
        WebView.RemoveScriptToExecuteOnDocumentCreated(identifier);
    }

    /// <summary>
    /// Posts a web message to the browser as a string.
    /// </summary>
    /// <param name="webMessageAsString">The message to post.</param>
    public void PostWebMessageAsString(string webMessageAsString)
    {
        WebView.PostWebMessageAsString(webMessageAsString);
    }

    /// <summary>
    /// Posts a web message to the browser as a JSON string.
    /// </summary>
    /// <param name="webMessageAsJson">The JSON message to post.</param>
    public void PostWebMessageAsJson(string webMessageAsJson)
    {
        WebView.PostWebMessageAsJson(webMessageAsJson);
    }

    /// <summary>
    /// Registers a native object to be accessible from JavaScript.
    /// </summary>
    /// <param name="name">The name of the object in JavaScript.</param>
    /// <param name="obj">The native object to register.</param>
    public void RegisterNativeObject(string name, NativeProxyObject obj)
    {
        WebView.RegisterNativeObject(name, obj);
    }

    /// <summary>
    /// Unregisters a previously registered native object.
    /// </summary>
    /// <param name="name">The name of the object to unregister.</param>
    public void UnregisterNativeObject(string name)
    {
        WebView.UnregisterNativeObject(name);
    }

    private WebViewCore? _webViewCore = null;

    /// <summary>
    /// Gets the underlying <see cref="WebViewCore"/> instance.
    /// </summary>
    //internal WebViewCore WebView { get; }
    internal WebViewCore WebView
    {
        get
        {
            if (_webViewCore is null)
            {

                _webViewCore = new WebViewCore(HostWindow, this)
                {
                    EnableOffscreenRendering = IsOffScreenRendering,
                    UseContextMenuStrip = IsOffScreenRendering,
                    SimplifyContextMenu = true,
                    BrowserWindowProc = BrowserWndProcCore,
                    OnConfigureBrowserSettings = OnConfigureBrowserSettingsCore,
                    WebMessageReceived = OnWebMessageReceivedCore,
                };
            }

            return _webViewCore;
        }
    }

    /// <summary>
    /// Called to configure browser settings before browser creation.
    /// </summary>
    /// <param name="browserSettings">The browser settings to configure.</param>
    protected virtual void OnConfigureBrowserSettings(CefBrowserSettings browserSettings)
    {
    }

    private bool _isSnapLayoutsRequired = false;

    /// <summary>
    /// Handles the core logic for when a web message is received.
    /// </summary>
    /// <param name="args">The event arguments containing the web message.</param>
    private void OnWebMessageReceivedCore(WebMessageReceivedEventArgs args)
    {
        if (HandleHostWindowMessages(args)) return;

        WebMessageReceived?.Invoke(this, args);
    }

    /// <summary>
    /// Handles special host window messages sent from JavaScript.
    /// </summary>
    /// <param name="e">The event arguments containing the web message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    private bool HandleHostWindowMessages(WebMessageReceivedEventArgs e)
    {
        var jsdoc = JsonDocument.Parse(e.WebMessageAsJson);
        if (jsdoc == null || jsdoc.RootElement.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (jsdoc.RootElement.TryGetProperty("passcode", out var elPasscode) && jsdoc.RootElement.TryGetProperty("message", out var elMessage))
        {
            var passcode = elPasscode.GetString();
            var name = elMessage.GetString();

            if (passcode != JS_MESSAGE_PASSCODE) return true;

            return InvokeIfRequired(() =>
            {
                switch (name)
                {
                    case "FormiumWindowCommand":
                        HandleJSWindowAppCommand(jsdoc.RootElement);
                        return true;

                    case "FormiumWindowMoveTo":
                        HandleJSWindowMoveTo(jsdoc.RootElement);
                        return true;

                    case "FormiumWindowMoveBy":
                        HandleJSWindowMoveBy(jsdoc.RootElement);
                        return true;

                    case "FormiumWindowResizeTo":
                        HandleJSWindowResizeTo(jsdoc.RootElement);
                        return true;

                    case "FormiumWindowResizeBy":
                        HandleJSWindowResizeBy(jsdoc.RootElement);
                        return true;

                    case "FormiumWindowSnapLayoutsRequired":
                        HandleJSWindowSnapLayoutsRequired(jsdoc.RootElement);
                        return true;
                }

                return false;
            });

            
        }
        return false;
    }

    /// <summary>
    /// Handles the JavaScript message for snap layouts requirement.
    /// </summary>
    /// <param name="jsonElement">The JSON element containing the message data.</param>
    private void HandleJSWindowSnapLayoutsRequired(JsonElement jsonElement)
    {
        if (!jsonElement.TryGetProperty("status", out var elStatus)) return;

        _isSnapLayoutsRequired = elStatus.GetBoolean();
    }

    /// <summary>
    /// Handles JavaScript window command messages (minimize, maximize, fullscreen, close).
    /// </summary>
    /// <param name="jsonElement">The JSON element containing the command.</param>
    private void HandleJSWindowAppCommand(JsonElement jsonElement)
    {
        if (!jsonElement.TryGetProperty("command", out var elCommand)) return;

        var command = elCommand.GetString();

        InvokeIfRequired(() =>
        {
            switch (command)
            {
                case "minimize":
                    WindowState = FormWindowState.Minimized;
                    break;

                case "maximize":
                    if (WindowState == FormWindowState.Maximized)
                    {
                        WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        WindowState = FormWindowState.Maximized;
                    }
                    break;
                case "fullscreen":
                    ToggleFullscreen();
                    break;
                case "close":
                    Close();
                    break;
            }
        });
    }

    /// <summary>
    /// Handles JavaScript message to resize the window by a delta.
    /// </summary>
    /// <param name="jsonElement">The JSON element containing the resize data.</param>
    private void HandleJSWindowResizeBy(JsonElement jsonElement)
    {
        if (!jsonElement.TryGetProperty("width", out var elX) || !jsonElement.TryGetProperty("width", out var elY)) return;

        var dx = elX.GetInt32();
        var dy = elY.GetInt32();

        Size = new Size(Width + dx, Height + dy);
    }

    /// <summary>
    /// Handles JavaScript message to resize the window to a specific size.
    /// </summary>
    /// <param name="jsonElement">The JSON element containing the resize data.</param>
    private void HandleJSWindowResizeTo(JsonElement jsonElement)
    {
        if (!jsonElement.TryGetProperty("width", out var elX) || !jsonElement.TryGetProperty("height", out var elY)) return;

        var dx = elX.GetInt32();
        var dy = elY.GetInt32();

        Size = new Size(dx, dy);
    }

    /// <summary>
    /// Handles JavaScript message to move the window by a delta.
    /// </summary>
    /// <param name="jsonElement">The JSON element containing the move data.</param>
    private void HandleJSWindowMoveBy(JsonElement jsonElement)
    {
        if (!jsonElement.TryGetProperty("x", out var elX) || !jsonElement.TryGetProperty("y", out var elY)) return;

        var dx = elX.GetInt32();
        var dy = elY.GetInt32();

        Location = new Point(Left + dx, Top + dy);
    }

    /// <summary>
    /// Handles JavaScript message to move the window to a specific location.
    /// </summary>
    /// <param name="jsonElement">The JSON element containing the move data.</param>
    private void HandleJSWindowMoveTo(JsonElement jsonElement)
    {
        if (!jsonElement.TryGetProperty("x", out var elX) || !jsonElement.TryGetProperty("y", out var elY)) return;

        var x = elX.GetInt32();
        var y = elY.GetInt32();

        Location = new Point(x, y);
    }

    private bool _isWindowActivated;

    /// <summary>
    /// Core window procedure handler.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    private bool WndProcCore(ref Message m)
    {
        if (IsOffScreenRendering && !WindowStyleSettings.HasSystemTitlebar && ProcessBorderlessBrowserMessage(ref m))
        {
            return true;
        }

        if((uint)m.Msg == WM_ACTIVATE)
        {
            _isWindowActivated = m.WParam != 0;

            if(WindowNativeObject is not null)
            {
                WindowNativeObject.Activated = _isWindowActivated;
            }
        }

        return WebView.HandleHostWndProc(ref m) || WndProc(ref m);
    }

    /// <summary>
    /// Core default window procedure handler.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    private bool DefWndProcCore(ref Message m) => DefWndProc(ref m);

    /// <summary>
    /// Core browser window procedure handler.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    private bool BrowserWndProcCore(ref Message m)
    {
        return (!WindowStyleSettings.HasSystemTitlebar && ProcessBorderlessBrowserMessage(ref m));
    }

    /// <summary>
    /// Configures browser settings before browser creation (internal use).
    /// </summary>
    /// <param name="browserSettings">The browser settings to configure.</param>
    private void OnConfigureBrowserSettingsCore(CefBrowserSettings browserSettings)
    {
        browserSettings.BackgroundColor = new CefColor(BackColor.A, BackColor.R, BackColor.G, BackColor.B);
        OnConfigureBrowserSettings(browserSettings);
    }

    /// <summary>
    /// Processes borderless browser messages for custom window handling.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    private bool ProcessBorderlessBrowserMessage(ref Message m)
    {
        var msg = (uint)m.Msg;
        var wParam = m.WParam;
        var lParam = m.LParam;

        var isDraggableArea = IsAppRegionDraggableArea(msg, lParam);

        switch (msg)
        {
            //case WM_MOUSEMOVE:
            //    return OnBrowserWmMouseMove(ref m);
            //case WM_SETCURSOR:
            //    return OnBrowserWmSetCursor(ref m);
            case WM_NCHITTEST:
                return OnBrowserWmHitTest(ref m, isDraggableArea);
            //case WM_NCLBUTTONDOWN:
            //    return OnBrowserWmNCLButtonDown(ref m);
            case WM_LBUTTONDOWN:
                return OnBrowserWmLButtonDown(ref m, isDraggableArea);

            case WM_RBUTTONUP when isDraggableArea && WindowStyleSettings.DisableAppRegionMenu == false:
                m.Result = 0;
                return OnBrowserWmRButtonAction(m);

            case WM_RBUTTONDOWN when isDraggableArea && WindowStyleSettings.DisableAppRegionMenu == false:
                //return OnBrowserWmRButtonAction(m, isDraggableArea);
                m.Result = 0;
                return true;

            case WM_LBUTTONDBLCLK:
                return OnBrowserWmLButtonDbClick(ref m, isDraggableArea);
        }

        return false;
    }

    /// <summary>
    /// Handles left mouse button down event in the browser window.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <param name="isDraggableArea">Indicates if the area is draggable.</param>
    /// <returns>True if the event was handled; otherwise, false.</returns>
    private bool OnBrowserWmLButtonDown(ref Message m, bool isDraggableArea)
    {
        if (isDraggableArea)
        {
            PostOrSendMessage(HWND, WM_SYSCOMMAND, SC_MOVE | HTCAPTION, m.LParam);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Handles right mouse button actions in the browser window.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <returns>True if the event was handled; otherwise, false.</returns>
    private bool OnBrowserWmRButtonAction(Message m)
    {
        InvokeIfRequired(() =>
        {
            var point = MARCOS.ToPoint(m.LParam);
            ClientToScreen(HWND, ref point);

            var hMenu = GetSystemMenu(HWND, false);

            var hCmd = TrackPopupMenuEx(hMenu, (uint)(TRACK_POPUP_MENU_FLAGS.TPM_RETURNCMD | TRACK_POPUP_MENU_FLAGS.TPM_TOPALIGN | TRACK_POPUP_MENU_FLAGS.TPM_LEFTALIGN), point.X, point.Y, HWND, null);

            PostOrSendMessage(HWND, WM_SYSCOMMAND, (nuint)hCmd.Value, m.LParam);
        });

        return true;
    }

    /// <summary>
    /// Posts or sends a Windows message depending on the rendering mode.
    /// </summary>
    /// <param name="hwnd">The window handle.</param>
    /// <param name="msg">The message ID.</param>
    /// <param name="wparam">The WPARAM value.</param>
    /// <param name="lparam">The LPARAM value.</param>
    private void PostOrSendMessage(HWND hwnd, uint msg, WPARAM wparam, LPARAM lparam)
    {
        if (IsOffScreenRendering)
        {
            SendMessage(hwnd, msg, wparam, lparam);
        }
        else
        {
            PostMessage(hwnd, msg, wparam, lparam);
        }
    }

    /// <summary>
    /// Called when the browser is created (internal use).
    /// </summary>
    /// <param name="browser">The created browser instance.</param>
    private void OnBrowserCreatedCore(CefBrowser browser)
    {
        HandleBrowserCreated(browser);

        OnBrowserCreated(browser);
    }

    /// <summary>
    /// Handles double-click event on the browser window's left mouse button.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <param name="isDraggableArea">Indicates if the area is draggable.</param>
    /// <returns>True if the event was handled; otherwise, false.</returns>
    private bool OnBrowserWmLButtonDbClick(ref Message m, bool isDraggableArea)
    {
        if (isDraggableArea && Maximizable && !Fullscreen)
        {
            InvokeIfRequired(() =>
            {
                if (WindowState == FormWindowState.Normal)
                {
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    WindowState = FormWindowState.Normal;
                }
            });

            return true;
        }

        return false;
    }

    /// <summary>
    /// Injects the host window script into the browser.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    private void InjectHostWindowScript(CefBrowser browser)
    {
        var script = Resources.Files.hostwindow_js;

        script = script.Replace("{{MESSAGE_PASSCODE}}", JS_MESSAGE_PASSCODE);
        script = script.Replace("{{HAS_TITLE_BAR}}", HasSystemTitlebar ? "true" : "false");

        var frame = browser.GetMainFrame();

        ExecuteJavaScript(frame, script);
    }

    /// <summary>
    /// Determines whether the specified message and location correspond to a draggable region.
    /// </summary>
    /// <param name="msg">The message ID.</param>
    /// <param name="lParam">The LPARAM value containing the point.</param>
    /// <returns>True if the region is draggable; otherwise, false.</returns>
    private bool IsAppRegionDraggableArea(uint msg, nint lParam)
    {
        uint[] filter = [WM_LBUTTONDOWN, WM_LBUTTONDBLCLK, WM_RBUTTONDOWN, WM_RBUTTONUP, WM_NCHITTEST];
        if (!filter.Contains(msg)) return false;

        var point = MARCOS.ToPoint(lParam);

        if (msg == WM_NCHITTEST)
        {
            ScreenToClient((HWND)Handle, ref point);
        }

        return IsDraggableRegion?.Invoke(point) ?? false;
    }

    /// <summary>
    /// Handles hit testing for the browser window.
    /// </summary>
    /// <param name="m">The Windows message.</param>
    /// <param name="isDraggableArea">Indicates if the area is draggable.</param>
    /// <returns>True if the event was handled; otherwise, false.</returns>
    private bool OnBrowserWmHitTest(ref Message m, bool isDraggableArea)
    {
        if (m.HWnd <= 0) return false;

        var point = MARCOS.ToPoint(m.LParam);

        ScreenToClient((HWND)m.HWnd, ref point);

        if (!HasSystemTitlebar && _isSnapLayoutsRequired)
        {
            m.Result = (nint)HTMAXBUTTON;
            return true;
        }

        if (isDraggableArea)
        {
            m.Result = (nint)HTCAPTION;
        }

        return false;
    }

    /// <summary>
    /// Sets a virtual host name to folder mapping for the browser.
    /// </summary>
    /// <param name="options">The folder mapping options.</param>
    public void SetVirtualHostNameToFolderMapping(FolderMappingOptions options)
    {
        WebView.SetVirtualHostNameToFolderMapping(options);
    }

    /// <summary>
    /// Sets a virtual host name to embedded file mapping for the browser.
    /// </summary>
    /// <param name="options">The embedded file mapping options.</param>
    public void SetVirtualHostNameToEmbeddedFileMapping(EmbeddedFileMappingOptions options)
    {
        WebView.SetVirtualHostNameToEmbeddedFileMapping(options);
    }

    /// <summary>
    /// Sets a virtual host name to server-sent events mapping for the browser.
    /// </summary>
    /// <param name="hostName">The virtual host name.</param>
    /// <returns>The <see cref="ServerSentEventsController"/> instance.</returns>
    public ServerSentEventsController SetVirtualHostNameToServerSentEvents(string hostName)
    {
        return WebView.SetVirtualHostNameToServerSentEvents(hostName);
    }

    /// <summary>
    /// Shows the browser's developer tools window.
    /// </summary>
    public void ShowDevTools()
    {
        WebView.ShowDevTools();
    }

    /// <summary>
    /// Hides the browser's developer tools window.
    /// </summary>
    public void HideDevTools()
    {
        WebView.HideDevTools();
    }

    //private void SetMouseCursor(uint mode)
    //{
    //    HCURSOR? handle = null;

    //    switch (mode)
    //    {
    //        case HTTOP:
    //        case HTBOTTOM:
    //            handle = LoadCursor(HINSTANCE.Null, IDC_SIZENS);
    //            break;
    //        case HTLEFT:
    //        case HTRIGHT:
    //            handle = LoadCursor(HINSTANCE.Null, IDC_SIZEWE);
    //            break;
    //        case HTTOPLEFT:
    //        case HTBOTTOMRIGHT:
    //            handle = LoadCursor(HINSTANCE.Null, IDC_SIZENWSE);

    //            break;
    //        case HTTOPRIGHT:
    //        case HTBOTTOMLEFT:
    //            handle = LoadCursor(HINSTANCE.Null, IDC_SIZENESW);
    //            break;
    //    }

    //    if (handle != null)
    //    {
    //        var oldCursor = SetCursor(handle.Value);
    //    }
    //}

    //private bool OnBrowserWmNCLButtonDown(ref Message m)
    //{
    //    if(m.WParam == HTCAPTION)
    //    {
    //        PostMessage(HWND, WM_SYSCOMMAND, SC_MOVE | HTCAPTION, m.LParam);
    //        return true;
    //    }

    //    return false;
    //}

    //private bool OnBrowserWmMouseMove(ref Message m)
    //{
    //    return false;
    //}

    //private bool OnBrowserWmSetCursor(ref Message m)
    //{
    //    if (WindowState != WindowState.Normal || !Resizable) return false;

    //    var pos = GetMessagePos();
    //    var point = MARCOS.ToPoint((nint)pos);

    //    var retval = HostedWindow.HitTestNCA(MARCOS.FromPoint(point));

    //    if (retval == HTNOWHERE || retval == HTCLIENT) return false;

    //    SetMouseCursor(retval);

    //    m.Result = 1;

    //    return true;

    //}

    //private bool OnBrowserWmMouseMove(ref Message m)
    //{
    //    if (WindowState != WindowState.Normal || !Resizable) return false;
    //    var pos = GetMessagePos();
    //    var point = MARCOS.ToPoint((nint)pos);
    //    var retval = HostedWindow.HitTestNCA(MARCOS.FromPoint(point));

    //    return retval != HTNOWHERE && retval != HTCLIENT;
    //}
}