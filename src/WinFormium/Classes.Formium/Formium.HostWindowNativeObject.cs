// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Exposes native window operations and properties to JavaScript for the Formium host window.
/// </summary>
internal class HostWindowNativeObject : JavaScriptNativeObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HostWindowNativeObject"/> class.
    /// Registers window properties and functions for JavaScript access.
    /// </summary>
    /// <param name="formium">The associated <see cref="Formium"/> instance.</param>
    public HostWindowNativeObject(Formium formium)
    {
        _formium = formium;

        //Activated = _formium.HostWindow.Focused;

        _formium.Activated += (_, _) => Activated = true;
        _formium.Deactivate += (_, _) => Activated = false;

        DefineProperty("activated", () => $"\"{Activated}\"".ToLower());
        DefineProperty("hasTitleBar", () => $"\"{HasTitleBar}\"".ToLower());
        DefineProperty("windowState", () => $"{_formium.WindowState}".ToLower());

        DefineProperty("x", () => $"{X}", v =>
        {
            if (int.TryParse(v, out var d))
            {
                _formium.InvokeIfRequired(() => _formium.Left = d);
            }
        });
        DefineProperty("y", () => $"{Y}", v =>
        {
            if (int.TryParse(v, out var d))
            {
                _formium.InvokeIfRequired(() => _formium.Top = d);
            }
        });
        DefineProperty("width", () => $"{Width}", v =>
        {
            if (int.TryParse(v, out var d))
            {
                _formium.InvokeIfRequired(() => _formium.Width = d);
            }
        });
        DefineProperty("height", () => $"{Height}", v =>
        {
            if (int.TryParse(v, out var d))
            {
                _formium.InvokeIfRequired(() => _formium.Height = d);
            }
        });

        DefineSynchronousFunction("minimize", (_) =>
        {
            Minimize();
            return null;
        });

        DefineSynchronousFunction("maximize", (_) =>
        {
            Maximize();
            return null;
        });

        DefineSynchronousFunction("restore", (_) =>
        {
            Restore();
            return null;
        });

        DefineSynchronousFunction("fullscreen", (_) =>
        {
            Fullscreen();
            return null;
        });

        DefineSynchronousFunction("toggleFullscreen", (_) =>
        {
            ToggleFullscreen();
            return null;
        });

        DefineSynchronousFunction("close", (_) =>
        {
            Close();
            return null;
        });

        DefineSynchronousFunction("activate", (_) =>
        {
            Activate();
            return null;
        });
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is currently activated (focused).
    /// </summary>
    public bool Activated { get; set; }

    /// <summary>
    /// Gets a value indicating whether the window has a system title bar.
    /// </summary>
    public bool HasTitleBar => _formium.HasSystemTitlebar;

    /// <summary>
    /// Gets the X coordinate of the window.
    /// </summary>
    public int X => _formium.Location.X;

    /// <summary>
    /// Gets the Y coordinate of the window.
    /// </summary>
    public int Y => _formium.Location.Y;

    /// <summary>
    /// Gets the width of the window.
    /// </summary>
    public int Width => _formium.Width;

    /// <summary>
    /// Gets the height of the window.
    /// </summary>
    public int Height => _formium.Height;

    /// <summary>
    /// Minimizes the window.
    /// </summary>
    public void Minimize()
    {
        if (_formium.InvokeRequired)
        {
            _formium.Invoke(Minimize);
            return;
        }

        _formium.WindowState = FormWindowState.Minimized;
    }

    /// <summary>
    /// Maximizes the window.
    /// </summary>
    public void Maximize()
    {
        if (_formium.InvokeRequired)
        {
            _formium.Invoke(Maximize);
            return;
        }

        _formium.WindowState = FormWindowState.Maximized;
    }

    /// <summary>
    /// Restores the window from minimized, maximized, or fullscreen state to normal.
    /// </summary>
    public void Restore()
    {
        if (_formium.InvokeRequired)
        {
            _formium.Invoke(Restore);
            return;
        }

        if (_formium.Fullscreen)
        {
            _formium.Fullscreen = false;
        }
        else
        {
            _formium.WindowState = FormWindowState.Normal;
        }
    }

    /// <summary>
    /// Sets the window to fullscreen mode.
    /// </summary>
    public void Fullscreen()
    {
        if (_formium.InvokeRequired)
        {
            _formium.Invoke(Fullscreen);
            return;
        }

        _formium.Fullscreen = true;
    }

    /// <summary>
    /// Toggles the window's fullscreen state.
    /// </summary>
    public void ToggleFullscreen()
    {
        _formium.Fullscreen = !_formium.Fullscreen;
    }

    /// <summary>
    /// Closes the window.
    /// </summary>
    public void Close()
    {
        if (_formium.InvokeRequired)
        {
            _formium.Invoke(Close);
            return;
        }

        _formium.Close();
    }

    /// <summary>
    /// Activates the window and brings it to the foreground, unless in fullscreen mode.
    /// </summary>
    public void Activate()
    {
        if (_formium.InvokeRequired)
        {
            _formium.Invoke(Activate);
            return;
        }

        if (_formium.Fullscreen) return;

        _formium.Activate();
    }

    /// <summary>
    /// The associated <see cref="Formium"/> instance.
    /// </summary>
    private readonly Formium _formium;
}