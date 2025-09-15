// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling drag-and-drop events in the browser.
/// </summary>
public interface IDragHandler
{
    /// <summary>
    /// Called when a drag operation enters the browser window.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance where the drag event occurred.</param>
    /// <param name="dragData">The <see cref="CefDragData"/> associated with the drag event.</param>
    /// <param name="mask">The allowed drag operations, as a <see cref="CefDragOperationsMask"/>.</param>
    /// <returns>
    /// <c>true</c> if the drag event is handled and the default behavior should be suppressed; otherwise, <c>false</c>.
    /// </returns>
    bool OnDragEnter(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask);

    /// <summary>
    /// Called when the draggable regions in the browser have changed.
    /// </summary>
    /// <param name="browser">The <see cref="CefBrowser"/> instance where the regions changed.</param>
    /// <param name="frame">The <see cref="CefFrame"/> associated with the change.</param>
    /// <param name="regions">An array of <see cref="CefDraggableRegion"/> objects representing the new draggable regions.</param>
    void OnDraggableRegionsChanged(CefBrowser browser, CefFrame frame, CefDraggableRegion[] regions);
}
