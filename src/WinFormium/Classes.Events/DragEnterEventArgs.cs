// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides data for the drag enter event in the browser.
/// </summary>
public class DragEnterEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DragEnterEventArgs"/> class.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance where the drag event occurred.</param>
    /// <param name="dragData">The <see cref="CefDragData"/> associated with the drag operation.</param>
    /// <param name="mask">The <see cref="CefDragOperationsMask"/> indicating allowed drag operations.</param>
    internal DragEnterEventArgs(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask)
    {
        Browser = browser;
        DragData = dragData;
        Mask = mask;
    }

    /// <summary>
    /// Gets the <see cref="CefBrowser"/> instance where the drag event occurred.
    /// </summary>
    public CefBrowser Browser { get; }

    /// <summary>
    /// Gets the <see cref="CefDragData"/> associated with the drag operation.
    /// </summary>
    public CefDragData DragData { get; }

    /// <summary>
    /// Gets the <see cref="CefDragOperationsMask"/> indicating allowed drag operations.
    /// </summary>
    public CefDragOperationsMask Mask { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the drag enter operation is allowed.
    /// </summary>
    public bool AllowDragEnter { get; set; } = false;
}
