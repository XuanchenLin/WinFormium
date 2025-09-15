// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Represents a delegate for handling Windows messages.
/// </summary>
/// <param name="m">A reference to the Windows message.</param>
/// <returns>True if the message was handled; otherwise, false.</returns>
public delegate bool WndProcDelegate(ref Message m);

/// <summary>
/// Provides an abstract base class for window settings in WinFormium.
/// </summary>
public abstract class WindowSettings
{
    /// <summary>
    /// Gets a value indicating whether off-screen rendering is enabled.
    /// </summary>
    protected internal abstract bool IsOffScreenRendering { get; }

    /// <summary>
    /// Gets a value indicating whether the window has a system title bar.
    /// </summary>
    protected internal abstract bool HasSystemTitlebar { get; }

    /// <summary>
    /// Gets or sets the custom window procedure delegate.
    /// </summary>
    protected internal abstract WndProcDelegate? WndProc { get; set; }

    /// <summary>
    /// Gets or sets the default window procedure delegate.
    /// </summary>
    protected internal abstract WndProcDelegate? DefWndProc { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window is in fullscreen mode.
    /// </summary>
    public virtual bool Fullscreen { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the window is resizable.
    /// </summary>
    public bool Resizable { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the menu of application region should be disabled.
    /// </summary>
    public bool DisableAppRegionMenu { get; set; } = false;

    /// <summary>
    /// Creates the host window for the current settings.
    /// </summary>
    /// <returns>A <see cref="Form"/> instance representing the host window.</returns>
    protected internal abstract Form CreateHostWindow();

    /// <summary>
    /// Configures additional properties for the specified WinForms form.
    /// </summary>
    /// <param name="form">The form to configure.</param>
    protected internal virtual void ConfigureWinFormProps(Form form)
    {
    }

    /// <summary>
    /// Gets the JavaScript code specified for this window, if any.
    /// </summary>
    protected internal virtual string? WindowSpecifiedJavaScript => null;

    /// <summary>
    /// Gets the custom render handler for this window, if any.
    /// Override this method to provide a custom <see cref="IRenderHandler"/> implementation
    /// for handling off-screen or windowless rendering scenarios.
    /// </summary>
    /// <returns>
    /// An <see cref="IOffscreenRender"/> instance if custom render is required.
    /// </returns>
    internal virtual IOffscreenRender GetOffscreenRender(FormiumOffscreenRenderHandler renderHandler) => null!;
}