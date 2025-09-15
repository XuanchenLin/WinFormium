// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.




namespace WinFormium;

/// <summary>
/// Provides settings specific to layered (offscreen rendered) windows in the WinFormium framework.
/// </summary>
/// <remarks>
/// Layered windows support offscreen rendering and do not support fullscreen mode or system titlebars.
/// </remarks>
public class LayeredWindowSettings : WindowSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the window is in fullscreen mode.
    /// For layered windows, fullscreen mode is not supported and setting this property has no effect.
    /// </summary>
    public override bool Fullscreen
    {
        get => _form?.Fullscreen ?? false;
        set
        {
            if (_form is not null)
            {
                _form.Fullscreen = value;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether offscreen rendering is enabled.
    /// Always returns <c>true</c> for layered windows.
    /// </summary>
    protected internal override bool IsOffScreenRendering => true;

    /// <summary>
    /// Gets a value indicating whether the window has a system titlebar.
    /// Always returns <c>false</c> for layered windows.
    /// </summary>
    internal protected override bool HasSystemTitlebar => false;

    /// <summary>
    /// Gets or sets the custom window procedure delegate for the layered window.
    /// </summary>
    internal protected override WndProcDelegate? WndProc { get; set; }

    /// <summary>
    /// Gets or sets the default window procedure delegate for the layered window.
    /// </summary>
    internal protected override WndProcDelegate? DefWndProc { get; set; }

    /// <summary>
    /// Gets or sets the padding offsets for the window edges.
    /// </summary>
    public Padding WindowEdgeOffsets { get; set; } = Padding.Empty;


    /// <summary>
    /// Gets or sets a value indicating whether the layered window allows mouse click-through.
    /// When set to <c>true</c>, mouse events will pass through the window to underlying windows.
    /// Default is <c>false</c>.
    /// </summary>
    public bool ClickThrough { get; set; } = false;

    /// <summary>
    /// Holds a reference to the associated <see cref="LayeredWindow"/> instance.
    /// </summary>
    private LayeredWindow? _form = null;

    /// <summary>
    /// Creates the host window for the layered window settings.
    /// </summary>
    /// <returns>A new instance of <see cref="LayeredWindow"/> configured with these settings.</returns>
    protected internal override Form CreateHostWindow()
    {
        var form = _form = new LayeredWindow(this)
        {

        };

        return form;
    }

    /// <summary>
    /// Gets the offscreen renderer for the layered window.
    /// </summary>
    /// <param name="renderHandler">The handler for offscreen rendering events.</param>
    /// <returns>An instance of <see cref="Direct2DOffscreenRender"/> for offscreen rendering.</returns>
    internal override IOffscreenRender GetOffscreenRender(FormiumOffscreenRenderHandler renderHandler)
    {
        return new Direct2DOffscreenRender(renderHandler);
    }
}


/// <summary>
/// Provides window settings for a kiosk-style window, which is typically fullscreen and borderless.
/// </summary>
public class KisokWindowSettings : WindowSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the window is in fullscreen mode.
    /// </summary>
    public override bool Fullscreen { get; set; }

    /// <summary>
    /// Gets a value indicating whether the system title bar is present. Always returns false for kiosk windows.
    /// </summary>
    protected internal override bool HasSystemTitlebar => false;

    /// <summary>
    /// Gets or sets the custom window procedure delegate.
    /// </summary>
    protected internal override WndProcDelegate? WndProc { get; set; }

    /// <summary>
    /// Gets or sets the default window procedure delegate.
    /// </summary>
    protected internal override WndProcDelegate? DefWndProc { get; set; }

    /// <summary>
    /// Gets or sets the target screen on which the kiosk window will be displayed.
    /// </summary>
    public Screen TargetScreen { get; set; } = Screen.PrimaryScreen!;

    /// <inheritdoc/>
    protected internal override bool IsOffScreenRendering => false;

    /// <summary>
    /// Creates the host window using the current kiosk window settings.
    /// </summary>
    /// <returns>A new <see cref="Form"/> instance configured as a kiosk window.</returns>
    protected internal override Form CreateHostWindow()
    {
        var form = new KisokWindow(this)
        {
            // Set properties for the KisokWindow
            FormBorderStyle = FormBorderStyle.None,
            StartPosition = FormStartPosition.Manual,
            WindowState = FormWindowState.Maximized,
            ShowInTaskbar = false,
            Bounds = TargetScreen.Bounds
        };

        return form;
    }

    /// <inheritdoc/>
    protected internal override void ConfigureWinFormProps(Form form)
    {
        form.ShowInTaskbar = false;

    }

    /// <summary>
    /// Represents the actual kiosk window form, configured according to <see cref="KisokWindowSettings"/>.
    /// </summary>
    class KisokWindow : Form
    {
        private Screen _screen;

        /// <summary>
        /// Initializes a new instance of the <see cref="KisokWindow"/> class with the specified settings.
        /// </summary>
        /// <param name="settings">The <see cref="KisokWindowSettings"/> to apply to the window.</param>
        public KisokWindow(KisokWindowSettings settings)
        {
            Settings = settings;
            // Initialize the window with the settings
            _screen = settings.TargetScreen;
        }

        /// <summary>
        /// Gets the <see cref="KisokWindowSettings"/> associated with this window.
        /// </summary>
        public KisokWindowSettings Settings { get; }

        /// <inheritdoc/>
        protected override void WndProc(ref Message m)
        {
            var wndProcs = Settings.WndProc?.GetInvocationList() ?? [];

            var result = false;

            foreach (WndProcDelegate wndProc in wndProcs)
            {
                result |= wndProc.Invoke(ref m);
            }

            if (result) return;

            base.WndProc(ref m);
        }

        /// <inheritdoc/>
        protected override void DefWndProc(ref Message m)
        {
            var wndProcs = Settings.DefWndProc?.GetInvocationList() ?? [];

            var result = false;
            foreach (WndProcDelegate wndProc in wndProcs)
            {
                result |= wndProc.Invoke(ref m);
            }

            if (result) return;

            base.DefWndProc(ref m);
        }
    }
}