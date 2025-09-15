// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

#region WinFormDesignerDisabler

internal partial class _WinFormDesignerDisabler
{ }

#endregion WinFormDesignerDisabler

/// <summary>
/// Represents the default window implementation for WinFormium, providing
/// initialization and message handling based on <see cref="DefaultWindowSettings"/>.
/// </summary>
internal class DefaultWindow : FormBase
{
    /// <summary>
    /// Stores the window settings used to configure this window instance.
    /// </summary>
    private readonly DefaultWindowSettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultWindow"/> class with the specified settings.
    /// </summary>
    /// <param name="settings">The settings to apply to the window.</param>
    public DefaultWindow(DefaultWindowSettings settings)
    {
        _settings = settings;
        ExtendsContentIntoTitleBar = settings.ExtendsContentIntoTitleBar;
        SystemBackdropType = settings.SystemBackdropType;
        SystemMenu = settings.SystemMenu;
        ShadowDecorated = settings.ShowWindowDecorators;
        WindowEdgeOffsets = settings.WindowEdgeOffsets;
        Resizable = settings.Resizable;
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