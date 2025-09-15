// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.





namespace WinFormium;


/// <summary>
/// Provides the default window settings for WinFormium windows, including system backdrop,
/// system menu, window decorators, and content extension into the title bar.
/// </summary>
public sealed class DefaultWindowSettings : WindowSettings
{

    /// <inheritdoc/>
    protected internal override bool IsOffScreenRendering => SystemBackdropType >= SystemBackdropType.None;

    /// <inheritdoc/>
    internal protected override bool HasSystemTitlebar => !ExtendsContentIntoTitleBar;

    /// <inheritdoc/>
    internal protected override WndProcDelegate? WndProc { get; set; }

    /// <inheritdoc/>
    internal protected override WndProcDelegate? DefWndProc { get; set; }

    /// <summary>
    /// Gets or sets the system backdrop type for the window.
    /// </summary>
    public SystemBackdropType SystemBackdropType
    {
        get; set;
    } = SystemBackdropType.Auto;

    /// <summary>
    /// Gets or sets a value indicating whether the system menu is enabled for the window.
    /// </summary>
    public bool SystemMenu
    {
        get; set;
    } = true;

    /// <summary>
    /// Gets or sets a value indicating whether window decorators (such as shadow or border) are shown.
    /// </summary>
    public bool ShowWindowDecorators { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the window content extends into the title bar area.
    /// </summary>
    public bool ExtendsContentIntoTitleBar { get; set; } = false;

    /// <summary>
    /// Gets or sets the window edge offsets for borderless resizing.
    /// </summary>
    public Padding WindowEdgeOffsets { get; set; } = Padding.Empty;

    /// <inheritdoc/>
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
    /// Holds a reference to the created <see cref="DefaultWindow"/> instance.
    /// </summary>
    private DefaultWindow? _form = null;

    /// <inheritdoc/>
    internal protected override Form CreateHostWindow()
    {
        var form = _form = new DefaultWindow(this)
        {

        };

        return form;
    }

    ///<inheritdoc/>
    internal override IOffscreenRender GetOffscreenRender(FormiumOffscreenRenderHandler renderHandler)
    {
        return new DirectCompositionOffscreenRender(renderHandler);
    }

}

