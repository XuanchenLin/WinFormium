// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
/// <summary>
/// Represents the base class for a Formium window, providing core window and browser functionality.
/// </summary>
public abstract partial class Formium
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Formium"/> class.
    /// Sets up the host window, configures window settings, initializes the WebView,
    /// and registers necessary event handlers and scripts.
    /// </summary>
    public Formium()
    {
        _hostWindowBuilder = new HostWindowBuilder();
        //WindowStyleSettings = ConfigureWindowSettings(_hostWindowBuilder);

        //HostWindow = WindowStyleSettings.CreateHostWindow();

        //ArgumentNullException.ThrowIfNull(HostWindow, nameof(HostWindow));

        //CreateWindow();

        //WebView = new WebViewCore(HostWindow, this)
        //{
        //    EnableOffscreenRendering = IsOffScreenRendering,
        //    UseContextMenuStrip = IsOffScreenRendering,
        //    SimplifyContextMenu = true,
        //    BrowserWindowProc = BrowserWndProcCore,
        //    OnConfigureBrowserSettings = OnConfigureBrowserSettingsCore,
        //    OnWebMessageReceived = OnWebMessageReceivedCore,
        //};
    }
}
