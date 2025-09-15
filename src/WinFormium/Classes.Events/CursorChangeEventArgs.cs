// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using Windows.Win32;

namespace WinFormium;

/// <summary>
/// Provides data for the cursor change event in the browser.
/// </summary>
public class CursorChangeEventArgs : EventArgs
{
    private nint cursorHandle;
    private CefCursorInfo customCursorInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="CursorChangeEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The browser where the cursor change occurred.</param>
    /// <param name="cursorHandle">The handle to the native cursor.</param>
    /// <param name="type">The type of the cursor.</param>
    /// <param name="customCursorInfo">The custom cursor information, if any.</param>
    internal CursorChangeEventArgs(CefBrowser browser, nint cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
    {
        Browser = browser;
        this.cursorHandle = cursorHandle;
        CursorType = type;
        this.customCursorInfo = customCursorInfo;
    }

    /// <summary>
    /// Gets the browser where the cursor change occurred.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the type of the cursor.
    /// </summary>
    public CefCursorType CursorType { get; }

    /// <summary>
    /// Gets the <see cref="Cursor"/> object representing the current cursor.
    /// </summary>
    /// <returns>The current <see cref="Cursor"/> instance.</returns>
    public Cursor GetCursor()
    {
        if (cursorHandle != 0 && CursorType != CefCursorType.None && CursorType != CefCursorType.Custom)
        {
            return new Cursor(cursorHandle);
        }
        else if (CursorType == CefCursorType.Custom)
        {
            using var buff = new MemoryStream(customCursorInfo.GetBuffer());
            var cursor = new Cursor(buff);
            return cursor;
        }
        return Cursors.Default;
    }

    /// <summary>
    /// Sets the system cursor to the specified <see cref="Cursor"/>.
    /// </summary>
    /// <param name="cursor">The cursor to set as the system cursor.</param>
    public void SetCursor(Cursor cursor)
    {
        PInvoke.SetCursor(new HCURSOR(cursor.Handle));
    }

    /// <summary>
    /// Gets or sets a value indicating whether the event is handled.
    /// </summary>
    public bool Handled { get; set; }
}
