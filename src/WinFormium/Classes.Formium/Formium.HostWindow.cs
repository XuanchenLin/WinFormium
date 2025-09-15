// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Represents the main window host for the Formium browser, providing window management and event handling.
/// </summary>
public partial class Formium : IWin32Window
{
    private readonly HostWindowBuilder _hostWindowBuilder;

    private Color _backgroundColor = Color.White;

    private string? _currentWindowStateString;

    private string _documentTitle = string.Empty;

    private bool _enabled = true;
    private Form? _hostWindow;

    private Icon? _icon = null;
    private bool _isDisposed = false;

    private Point? _location;
    private bool _maximizable = true;
    private Size? _maximumSize;
    private bool _minimizable = true;
    private Size? _minimumSize;
    private bool _showDocumentTitle = false;

    private bool _showInTaskbar = true;
    private Size? _size;
    private FormStartPosition _startPosition = FormStartPosition.WindowsDefaultLocation;
    private bool _topMost = false;
    private bool _visible = true;
    private string _windowCaption = "WinFomrium";

    private FormWindowState _windowState = FormWindowState.Normal;
    private WindowSettings? _windowStyleSettings = null;

    /// <summary>
    /// Occurs when the window is activated.
    /// </summary>
    public event EventHandler? Activated;

    /// <summary>
    /// Occurs when the window is deactivated.
    /// </summary>
    public event EventHandler? Deactivate;

    /// <summary>
    /// Occurs when the window has closed.
    /// </summary>
    public event FormClosedEventHandler? FormClosed;

    /// <summary>
    /// Occurs when the window is closing.
    /// </summary>
    public event FormClosingEventHandler? FormClosing;

    /// <summary>
    /// Occurs when the window receives focus.
    /// </summary>
    public event EventHandler? GotFocus;

    /// <summary>
    /// Occurs when the window loses focus.
    /// </summary>
    public event EventHandler? LostFocus;

    /// <summary>
    /// Occurs when the window is moved.
    /// </summary>
    public event EventHandler? Move;

    /// <summary>
    /// Occurs when the window is resized.
    /// </summary>
    public event EventHandler? Resize;

    /// <summary>
    /// Occurs when the window resize operation begins.
    /// </summary>
    public event EventHandler? ResizeBegin;

    /// <summary>
    /// Occurs when the window resize operation ends.
    /// </summary>
    public event EventHandler? ResizeEnd;

    /// <summary>
    /// Occurs when the window is shown.
    /// </summary>
    public event EventHandler? Shown;

    /// <summary>
    /// Occurs when the window's visibility changes.
    /// </summary>
    public event EventHandler? VisibleChanged;

    /// <summary>
    /// Gets or sets a value indicating whether fullscreen is allowed.
    /// </summary>
    public bool AllowFullscreen { get; set; }

    /// <summary>
    /// Gets or sets the background color of the window.
    /// </summary>
    public Color BackColor
    {
        get
        {
            return _backgroundColor;
        }
        set
        {
            _backgroundColor = value;

            if (IsWindowCreated)
            {
                HostWindow.BackColor = Color.FromArgb(255, _backgroundColor);
            }
        }
    }

    /// <summary>
    /// Gets the current window caption.
    /// </summary>
    public string Caption
    {
        get
        {
            var pageTitle = $"{DocumentTitle}".Trim();
            if (ShowDocumentTitle && !string.IsNullOrWhiteSpace(pageTitle))
            {
                return string.Format(CaptionPattern, pageTitle, WindowTitle);
            }
            else
            {
                return WindowTitle;
            }
        }
    }

    /// <summary>
    /// Gets the caption pattern used for the window title.
    /// </summary>
    public virtual string CaptionPattern => "{0} - {1}";

    /// <summary>
    /// Gets or sets the document title.
    /// </summary>
    public string DocumentTitle
    {
        get => _documentTitle;
        internal set
        {
            _documentTitle = value;

            if (IsWindowCreated)
            {
                UpdateWindowCaption();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is enabled.
    /// </summary>
    public bool Enabled
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.Enabled;
            }
            return _enabled;
        }
        set
        {
            _enabled = value;

            if (IsWindowCreated)
            {
                HostWindow.Enabled = _enabled;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is in fullscreen mode.
    /// </summary>
    public bool Fullscreen
    {
        get => WindowStyleSettings.Fullscreen;
        set
        {
            if (!AllowFullscreen) return;

            WindowStyleSettings.Fullscreen = value;
        }
    }

    /// <summary>
    /// Gets the window handle.
    /// </summary>
    public nint Handle { get; private set; }

    /// <summary>
    /// Gets or sets the height of the window.
    /// </summary>
    public int Height
    {
        get => _hostWindow?.Height ?? 0;
        set
        {
            if (IsWindowCreated)
            {
                HostWindow.Height = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the window icon.
    /// </summary>
    public Icon? Icon
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.Icon;
            }
            return _icon;
        }
        set
        {
            _icon = value;

            if (IsWindowCreated)
            {
                HostWindow.Icon = value;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether invoke is required for thread safety.
    /// </summary>
    public bool InvokeRequired => _hostWindow?.InvokeRequired ?? false;

    /// <summary>
    /// Gets a value indicating whether the host window is disposed.
    /// </summary>
    public bool IsDisposed => _hostWindow?.IsDisposed ?? false || _isDisposed;

    /// <summary>
    /// Gets or sets the left position of the window.
    /// </summary>
    public int Left
    {
        get => _hostWindow?.Left ?? 0;
        set
        {
            if (IsWindowCreated)
            {
                HostWindow.Left = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the location of the window.
    /// </summary>
    public Point Location
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.Location;
            }
            return _location ?? default;
        }
        set
        {
            _location = value;

            if (IsWindowCreated)
            {
                HostWindow.Location = _location.Value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window can be maximized.
    /// </summary>
    public bool Maximizable
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.MaximizeBox;
            }
            return _maximizable;
        }
        set
        {
            _maximizable = value;

            if (IsWindowCreated)
            {
                HostWindow.MaximizeBox = _maximizable;
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum size of the window.
    /// </summary>
    public Size MaximumSize
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.MaximumSize;
            }
            return _maximumSize ?? default;
        }
        set
        {
            _maximumSize = value;

            if (IsWindowCreated)
            {
                HostWindow.MaximumSize = _maximumSize.Value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window can be minimized.
    /// </summary>
    public bool Minimizable
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.MinimizeBox;
            }
            return _minimizable;
        }
        set
        {
            _minimizable = value;

            if (IsWindowCreated)
            {
                HostWindow.MinimizeBox = _minimizable;
            }
        }
    }

    /// <summary>
    /// Gets or sets the minimum size of the window.
    /// </summary>
    public Size MinimumSize
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.MinimumSize;
            }
            return _minimumSize ?? default;
        }
        set
        {
            _minimumSize = value;

            if (IsWindowCreated)
            {
                HostWindow.MinimumSize = _minimumSize.Value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show the document title in the window caption.
    /// </summary>
    public bool ShowDocumentTitle
    {
        get => _showDocumentTitle;
        set
        {
            _showDocumentTitle = value;

            if (IsWindowCreated)
            {
                UpdateWindowCaption();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is shown in the taskbar.
    /// </summary>
    public bool ShowInTaskbar
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.ShowInTaskbar;
            }
            return _showInTaskbar;
        }
        set
        {
            _showInTaskbar = value;

            if (IsWindowCreated)
            {
                HostWindow.ShowInTaskbar = _showInTaskbar;
            }
        }
    }

    /// <summary>
    /// Gets or sets the size of the window.
    /// </summary>
    public Size Size
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.Size;
            }
            return _size ?? default;
        }
        set
        {
            _size = value;

            if (IsWindowCreated)
            {
                HostWindow.Size = _size.Value;
            }

        }
    }

    /// <summary>
    /// Gets or sets the start position of the window.
    /// </summary>
    public FormStartPosition StartPosition
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.StartPosition;
            }
            return _startPosition;
        }
        set
        {
            _startPosition = value;

            if (IsWindowCreated)
            {
                HostWindow.StartPosition = _startPosition;
            }
        }
    }

    /// <summary>
    /// Gets or sets the top position of the window.
    /// </summary>
    public int Top
    {
        get => _hostWindow?.Top ?? 0;
        set
        {
            if (IsWindowCreated)
            {
                HostWindow.Top = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is topmost.
    /// </summary>
    public bool TopMost
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.TopMost;
            }
            return _topMost;
        }
        set
        {
            _topMost = value;

            if (IsWindowCreated)
            {
                HostWindow.TopMost = _topMost;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is visible.
    /// </summary>
    public bool Visible
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.Visible;
            }
            return _visible;
        }
        set
        {
            _visible = value;

            if (IsWindowCreated)
            {
                HostWindow.Visible = _visible;
            }
        }
    }

    /// <summary>
    /// Gets or sets the width of the window.
    /// </summary>
    public int Width
    {
        get => _hostWindow?.Width ?? 0;
        set
        {
            if (IsWindowCreated)
            {
                HostWindow.Width = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the window state (normal, minimized, maximized).
    /// </summary>
    public FormWindowState WindowState
    {
        get
        {
            if (IsWindowCreated)
            {
                return HostWindow.WindowState;
            }

            return _windowState;
        }
        set
        {
            if (Fullscreen) return;

            if (IsWindowCreated)
            {
                HostWindow.WindowState = value;
            }
            else
            {
                _windowState = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the window caption.
    /// </summary>
    public string WindowTitle
    {
        get => _windowCaption;
        set
        {
            _windowCaption = value;

            if (IsWindowCreated)
            {
                UpdateWindowCaption();
            }
        }
    }
    /// <summary>
    /// Gets the internal host window instance.
    /// </summary>
    internal Form HostWindow
    {
        get
        {
            if (_hostWindow is null)
            {
                CreateWindow();
            }
            return _hostWindow!;
        }
    }

    /// <summary>
    /// Gets the window handle as an HWND.
    /// </summary>
    internal HWND HWND => (HWND)Handle;

    /// <summary>
    /// Gets a value indicating whether off-screen rendering is enabled.
    /// </summary>
    internal bool IsOffScreenRendering => WindowStyleSettings.IsOffScreenRendering;

    internal bool IsWindowCreated => _hostWindow != null;

    internal bool IsBrowserCreated => WebView.Initialized && WebView.Browser is not null;

    /// <summary>
    /// Gets a value indicating whether the window is ready for interaction, meaning both the host window and browser are created.
    /// </summary>
    public bool Ready => IsWindowCreated && IsBrowserCreated;

    /// <summary>
    /// Initializes a new instance of the <see cref="Formium"/> class with the specified host window builder options.
    /// </summary>
    internal WindowSettings WindowStyleSettings
    {
        get
        {
            if (_windowStyleSettings is null)
            {
                _windowStyleSettings = ConfigureWindowSettings(_hostWindowBuilder);
            }

            return _windowStyleSettings;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window has a system title bar.
    /// </summary>
    protected internal bool HasSystemTitlebar => WindowStyleSettings.HasSystemTitlebar;

    /// <summary>
    /// Gets the DPI scale factor for the window.
    /// </summary>
    protected float DpiScaleFactor
    {
        get
        {
            var dpi = 96;

            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 14393))
            {
                dpi = (int)GetDpiForWindow(HWND);

                return dpi / 96f;
            }

            return dpi / 96f;
        }
    }

    /// <summary>
    /// Activates the window and sets focus to the browser if initialized.
    /// </summary>
    public void Activate()
    {
        HostWindow.Activate();
    }

    /// <summary>
    /// Closes the window.
    /// </summary>
    public void Close()
    {
        HostWindow.Close();
    }

    /// <summary>
    /// Invokes the specified action on the UI thread.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    public void Invoke(Action action) => HostWindow.Invoke(action);

    /// <summary>
    /// Invokes the specified delegate on the UI thread.
    /// </summary>
    /// <param name="action">The delegate to invoke.</param>
    /// <returns>The result of the delegate.</returns>
    public object Invoke(Delegate action) => HostWindow.Invoke(action);

    /// <summary>
    /// Invokes the specified delegate with arguments on the UI thread.
    /// </summary>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="args">The arguments for the delegate.</param>
    /// <returns>The result of the delegate.</returns>
    public object Invoke(Delegate method, params object[]? args) => HostWindow.Invoke(method, args);

    /// <summary>
    /// Invokes the specified function on the UI thread and returns the result.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="func">The function to invoke.</param>
    /// <returns>The result of the function.</returns>
    public T? Invoke<T>(Func<T> func) => HostWindow.Invoke(func);

    /// <summary>
    /// Invokes the specified action on the UI thread if required.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    public void InvokeIfRequired(Action action)
    {
        if (IsDisposed) return;

        if (HostWindow.InvokeRequired)
        {
            try
            {
                HostWindow.Invoke(action);
            }
            catch (ObjectDisposedException) { }
            catch (ThreadAbortException) { }
        }
        else
        {
            action();
        }
    }

    /// <summary>
    /// Invokes the specified delegate with arguments on the UI thread if required.
    /// </summary>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="args">The arguments for the delegate.</param>
    /// <returns>The result of the delegate.</returns>
    public object? InvokeIfRequired(Delegate method, params object[] args)
    {
        if (HostWindow.IsDisposed) return default;


        if (HostWindow.InvokeRequired)
            try
            {
                return HostWindow.Invoke(method, args);
            }
            catch (ObjectDisposedException) { }
            catch (ThreadAbortException) { }
        else
            return method.DynamicInvoke(args);

        return default;
    }

    /// <summary>
    /// Invokes the specified delegate with arguments on the UI thread if required and returns the result.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="method">The delegate to invoke.</param>
    /// <param name="args">The arguments for the delegate.</param>
    /// <returns>The result of the delegate.</returns>
    public T? InvokeIfRequired<T>(Delegate method, params object[] args)
    {
        if (HostWindow.IsDisposed) return default;

        if (HostWindow.InvokeRequired)
            try
            {
                return (T?)HostWindow.Invoke(method, args);
            }
            catch (ObjectDisposedException) { }
            catch (ThreadAbortException) { }
        else
            return (T?)method.DynamicInvoke(args);
        return default;

    }

    /// <summary>
    /// Invokes the specified function on the UI thread if required and returns the result.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="func">The function to invoke.</param>
    /// <returns>The result of the function.</returns>
    public T? InvokeIfRequired<T>(Func<T> func)
    {
        if (HostWindow.IsDisposed) return default;

        if (HostWindow.InvokeRequired)
            try
            {
                return HostWindow.Invoke(func);
            }
            catch (ObjectDisposedException) { }
            catch (ThreadAbortException) { }
        else
            return func();
        return default;
    }

    /// <summary>
    /// Shows the window.
    /// </summary>
    public void Show()
    {
        HostWindow.Show();
    }

    /// <summary>
    /// Shows the window with the specified owner.
    /// </summary>
    /// <param name="owner">The owner window.</param>
    public void Show(IWin32Window owner)
    {
        AssignOwnerFromHandle(HostWindow, owner);

        HostWindow.Show(owner);
    }

    /// <summary>
    /// Shows the window as a modal dialog.
    /// </summary>
    /// <returns>The dialog result.</returns>
    public DialogResult ShowDialog()
    {
        return HostWindow.ShowDialog();
    }

    /// <summary>
    /// Shows the window as a modal dialog with the specified owner.
    /// </summary>
    /// <param name="owner">The owner window.</param>
    /// <returns>The dialog result.</returns>
    public DialogResult ShowDialog(IWin32Window owner)
    {

        AssignOwnerFromHandle(HostWindow, owner);

        return HostWindow.ShowDialog(owner);
    }

    /// <summary>
    /// Toggles the fullscreen state of the window.
    /// </summary>
    public void ToggleFullscreen()
    {
        Fullscreen = !Fullscreen;
    }

    /// <summary>
    /// Updates the window caption based on the current settings.
    /// </summary>
    internal void UpdateWindowCaption()
    {
        if (HostWindow.InvokeRequired)
        {
            HostWindow.Invoke(UpdateWindowCaption);
            return;
        }

        HostWindow.Text = Caption;
    }

    /// <summary>
    /// Configures the window settings. Override to provide custom window settings.
    /// </summary>
    /// <param name="opts">The host window builder options.</param>
    /// <returns>The configured window settings.</returns>
    protected internal virtual WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
    {
        return opts.UseDefaultWindow();
    }

    /// <summary>
    /// Processes default window messages. Override to handle custom messages.
    /// </summary>
    /// <param name="m">The window message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    protected virtual bool DefWndProc(ref Message m)
    {
        return false;
    }

    /// <summary>
    /// Called when the window is activated.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnActivated(object? sender, EventArgs e)
    {
        Activated?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the window is deactivated.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnDeactivate(object? sender, EventArgs e)
    {
        Deactivate?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the window has closed.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnFormClosed(object? sender, FormClosedEventArgs e)
    {
        _isDisposed = true;
        FormClosed?.Invoke(this, e);
    }

    /// <summary>
    /// Called when the window is closing.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnFormClosing(object? sender, FormClosingEventArgs e)
    {
        FormClosing?.Invoke(this, e);
    }

    /// <summary>
    /// Called when the window receives focus.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnGotFocus(object? sender, EventArgs e)
    {
        GotFocus?.Invoke(this, e);
    }

    /// <summary>
    /// Called when the window loses focus.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnLostFocus(object? sender, EventArgs e)
    {
        LostFocus?.Invoke(this, e);
    }

    /// <summary>
    /// Called when the window is moved.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnMove(object? sender, EventArgs e)
    {
        Move?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the window is resized.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnResize(object? sender, EventArgs e)
    {
        Resize?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the window resize operation begins.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnResizeBegin(object? sender, EventArgs e)
    {
        ResizeBegin?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the window resize operation ends.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnResizeEnd(object? sender, EventArgs e)
    {
        ResizeEnd?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the window is shown.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnShown(object? sender, EventArgs e)
    {
        Shown?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when the window's visibility changes.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnVisibleChanged(object? sender, EventArgs e)
    {
        VisibleChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Processes window messages. Override to handle custom messages.
    /// </summary>
    /// <param name="m">The window message.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    protected virtual bool WndProc(ref Message m)
    {
        return false;
    }

    /// <summary>
    /// Assigns the owner of the child form based on the provided handle.
    /// </summary>
    /// <param name="child">
    /// The child form to assign the owner to.
    /// </param>
    /// <param name="owner">
    /// The owner window handle to assign as the owner of the child form.
    /// </param>
    private void AssignOwnerFromHandle(Form child, IWin32Window owner)
    {
        if (owner is not null)
        {
            var forms = Application.OpenForms.Cast<Form>();

            var ownerForm = forms.SingleOrDefault(x => x.Handle == owner.Handle);

            child.Owner = ownerForm;
        }
    }

    private void CreateWindow()
    {
        _hostWindow = WindowStyleSettings.CreateHostWindow();

        _hostWindow.MinimizeBox = _minimizable;
        _hostWindow.MaximizeBox = _maximizable;
        _hostWindow.ShowInTaskbar = _showInTaskbar;
        _hostWindow.StartPosition = _startPosition;
        _hostWindow.TopMost = _topMost;
        _hostWindow.Text = _windowCaption;
        _hostWindow.Enabled = _enabled;
        if (_icon is not null)
        {
            _hostWindow.Icon = _icon;
        }



        if (_minimumSize.HasValue)
        {
            _hostWindow.MinimumSize = _minimumSize.Value;
        }

        if (_maximumSize.HasValue)
        {
            _hostWindow.MaximumSize = _maximumSize.Value;
        }

        if (_size.HasValue)
        {
            _hostWindow.Size = _size.Value;
        }

        if (_location.HasValue)
        {
            _hostWindow.Location = _location.Value;
        }

        _hostWindow.BackColor = Color.FromArgb(255, _backgroundColor);


        WindowStyleSettings.WndProc += WndProcCore;
        WindowStyleSettings.DefWndProc += DefWndProcCore;

        _hostWindow.HandleCreated += (_, _) =>
        {
            Handle = HostWindow.Handle;

            if (WebView.Initialized && WindowStyleSettings.WindowSpecifiedJavaScript is not null)
            {
                WebView.AddScriptToExecuteOnDocumentCreated(WindowStyleSettings.WindowSpecifiedJavaScript);
            }
        };

        WindowStyleSettings.ConfigureWinFormProps(_hostWindow);


        RegisterHostWindowEvents();
    }

    HostWindowNativeObject? WindowNativeObject { get; set; }

    private void HandleBrowserCreated(CefBrowser browser)
    {
        RegisterNativeObject("__hostWindowObject", WindowNativeObject = new HostWindowNativeObject(this)
        {
            Activated = _isWindowActivated
        });


        //UpdateWindowCaption();
        //Activate();
    }

    private void OnActivatedCore(object? sender, EventArgs e)
    {
        OnActivated(this, e);

        PostActivatedMessage(true);
    }

    private void OnDeactivateCore(object? sender, EventArgs e)
    {
        OnDeactivate(this, e);

        PostActivatedMessage(false);
    }

    private void PostActivatedMessage(bool isActivated)
    {
        if (!WebView.Initialized) return;

        PostWebMessageAsJson(JsonSerializer.Serialize(new JsonNotifyWindowActivated(JS_MESSAGE_PASSCODE, "FormiumNotifyWindowActivated", isActivated), WinFormiumJsonSerializerContext.Default.JsonNotifyWindowActivated));
    }

    private void OnFormClosedCore(object? sender, FormClosedEventArgs e)
    {

        if (HostWindow.Owner is not null && HostWindow.Owner.OwnedForms.Length == 1)
        {
            HostWindow.Owner.Activate();
        }


        OnFormClosed(this, e);


    }
    private void OnMoveCore(object? sender, EventArgs e)
    {
        OnMove(this, e);

        if (!WebView.Initialized) return;

        var screen = Screen.FromHandle(Handle);

        var x = HostWindow.Left;
        var y = HostWindow.Top;
        var scrX = x - screen.Bounds.X;
        var scrY = y - screen.Bounds.Y;

        PostWebMessageAsJson(JsonSerializer.Serialize(new JsonNotifyWindowMove(JS_MESSAGE_PASSCODE, "FormiumNotifyWindowMove", x, y, scrX, scrY), WinFormiumJsonSerializerContext.Default.JsonNotifyWindowMove));
    }

    private void OnResizeBeginCore(object? sender, EventArgs e)
    {
        OnResizeBegin(this, e);
    }

    private void OnResizeCore(object? sender, EventArgs e)
    {
        OnResize(this, e);

        if (!WebView.Initialized) return;

        var state = WindowState.ToString().ToLower();

        if (_currentWindowStateString is null)
        {
            _currentWindowStateString = state;
        }

        if (Fullscreen && _currentWindowStateString != $"{nameof(Fullscreen)}".ToLower())
        {
            state = nameof(Fullscreen).ToLower();
        }

        if (_currentWindowStateString != state)
        {
            _currentWindowStateString = state;

            PostWebMessageAsJson(JsonSerializer.Serialize(new JsonNotifyWindowStateChange(JS_MESSAGE_PASSCODE, "FormiumNotifyWindowStateChange", _currentWindowStateString), WinFormiumJsonSerializerContext.Default.JsonNotifyWindowStateChange));
        }

        PostWebMessageAsJson(JsonSerializer.Serialize(new JsonNotifyWindowResize(JS_MESSAGE_PASSCODE, "FormiumNotifyWindowResize", HostWindow.Left, HostWindow.Top, HostWindow.Width, HostWindow.Height), WinFormiumJsonSerializerContext.Default.JsonNotifyWindowResize));
    }

    private void OnResizeEndCore(object? sender, EventArgs e)
    {
        OnResizeEnd(this, e);
    }

    /// <summary>
    /// Registers host window events to internal handlers.
    /// </summary>
    private void RegisterHostWindowEvents()
    {
        HostWindow.Activated += OnActivatedCore;
        HostWindow.Deactivate += OnDeactivateCore;
        HostWindow.ResizeBegin += OnResizeBeginCore;
        HostWindow.Resize += OnResizeCore;
        HostWindow.ResizeEnd += OnResizeEndCore;
        HostWindow.VisibleChanged += OnVisibleChanged;
        HostWindow.Move += OnMoveCore;
        HostWindow.Shown += OnShown;
        HostWindow.FormClosing += OnFormClosing;
        HostWindow.FormClosed += OnFormClosedCore;
    }
}

/// <summary>
/// Represents a notification for window state change.
/// </summary>
internal record JsonNotifyWindowStateChange(string Passcode, string Message, string State);
/// <summary>
/// Represents a notification for window resize.
/// </summary>
internal record JsonNotifyWindowResize(string Passcode, string Message, int X, int Y, int Width, int Height);
/// <summary>
/// Represents a notification for window move.
/// </summary>
internal record JsonNotifyWindowMove(string Passcode, string Message, int X, int Y, int ScreenX, int ScreenY);
/// <summary>
/// Represents a notification for window activation state.
/// </summary>
internal record JsonNotifyWindowActivated(string Passcode, string Message, bool State);