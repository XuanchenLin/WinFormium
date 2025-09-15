// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using WinFormium.Browser.CefGlue.Platform.Windows;

namespace WinFormium;

#region WinFormDesignerDisabler

internal partial class _WinFormDesignerDisabler
{ }

#endregion WinFormDesignerDisabler

/// <summary>
/// Represents a window with layered style, supporting custom window procedures and settings.
/// </summary>
internal class LayeredWindow : FormBase
{
    /// <summary>
    /// Stores the settings for the layered window, including custom window procedure delegates.
    /// </summary>
    private readonly LayeredWindowSettings _settings;
    private readonly bool _clickThrough;

    /// <summary>
    /// Gets the parameters required to create the window, adding the WS_EX_LAYERED extended style.
    /// </summary>
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;

            cp.ExStyle |= (int)(WindowStyleEx.WS_EX_LAYERED);

            if(_clickThrough)
            {
                cp.ExStyle |= (int)(WindowStyleEx.WS_EX_TRANSPARENT);
            }

            return cp;
        }
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="LayeredWindow"/> class with the specified settings.
    /// </summary>
    /// <param name="settings">The settings to configure the layered window.</param>
    public LayeredWindow(LayeredWindowSettings settings)
    {
        _settings = settings;
        ExtendsContentIntoTitleBar = true;
        ShadowDecorated = false;
        Resizable = settings.Resizable;
        _clickThrough = settings.ClickThrough;
    }


    /// <inheritdoc/>
    protected override void WndProc(ref Message m)
    {
        var wndProcs = _settings.WndProc?.GetInvocationList() ?? [];

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
        var wndProcs = _settings.DefWndProc?.GetInvocationList() ?? [];

        var result = false;
        foreach (WndProcDelegate wndProc in wndProcs)
        {
            result |= wndProc.Invoke(ref m);
        }

        if (result) return;

        base.DefWndProc(ref m);
    }
}