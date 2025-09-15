// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
/// <summary>
/// Interface for handling rendering and related events for a browser instance.
/// Implement this interface to provide custom rendering, accessibility, drag-and-drop,
/// touch, IME, and virtual keyboard support for windowless browsers.
/// </summary>
public interface IRenderHandler
{
    /// <summary>
    /// Returns the accessibility handler for the browser, or null if not implemented.
    /// </summary>
    /// <returns>The <see cref="CefAccessibilityHandler"/> instance or null.</returns>
    CefAccessibilityHandler? GetAccessibilityHandler();

    /// <summary>
    /// Retrieve the root screen rectangle in device coordinates.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="rect">The rectangle to be set with the root screen coordinates.</param>
    /// <returns>True if the rectangle was provided, false otherwise.</returns>
    bool GetRootScreenRect(CefBrowser browser, ref CefRectangle rect);

    /// <summary>
    /// Retrieve the view rectangle in device coordinates.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="rect">The rectangle to be set with the view coordinates.</param>
    void GetViewRect(CefBrowser browser, out CefRectangle rect);

    /// <summary>
    /// Convert view coordinates to screen coordinates.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="viewX">X coordinate in view space.</param>
    /// <param name="viewY">Y coordinate in view space.</param>
    /// <param name="screenX">Resulting X coordinate in screen space.</param>
    /// <param name="screenY">Resulting Y coordinate in screen space.</param>
    /// <returns>True if the conversion was successful, false otherwise.</returns>
    bool GetScreenPoint(CefBrowser browser, int viewX, int viewY, ref int screenX, ref int screenY);

    /// <summary>
    /// Provide screen information for the browser.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="screenInfo">The screen information to be filled.</param>
    /// <returns>True if the information was provided, false otherwise.</returns>
    bool GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo);

    /// <summary>
    /// Called when a popup is shown or hidden.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="show">True if the popup is shown, false if hidden.</param>
    void OnPopupShow(CefBrowser browser, bool show);

    /// <summary>
    /// Called when the popup size or position changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="rect">The new popup rectangle.</param>
    void OnPopupSize(CefBrowser browser, CefRectangle rect);

    /// <summary>
    /// Called when the browser needs to be repainted.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="type">The paint element type.</param>
    /// <param name="dirtyRects">Array of dirty rectangles that need repainting.</param>
    /// <param name="buffer">Pointer to the pixel buffer.</param>
    /// <param name="width">Width of the buffer.</param>
    /// <param name="height">Height of the buffer.</param>
    void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, nint buffer, int width, int height);

    /// <summary>
    /// Called when accelerated painting is requested.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="type">The paint element type.</param>
    /// <param name="dirtyRects">Array of dirty rectangles that need repainting.</param>
    /// <param name="sharedHandle">Handle to the shared resource for accelerated painting.</param>
    void OnAcceleratedPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, nint sharedHandle);

    /// <summary>
    /// Retrieve the size of the touch handle for the specified orientation.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="orientation">The orientation of the touch handle.</param>
    /// <param name="size">The size to be set for the touch handle.</param>
    void GetTouchHandleSize(CefBrowser browser, CefHorizontalAlignment orientation, out CefSize size);

    /// <summary>
    /// Called when the state of a touch handle changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="state">The new touch handle state.</param>
    void OnTouchHandleStateChanged(CefBrowser browser, CefTouchHandleState state);

    /// <summary>
    /// Called when a drag operation is started.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="dragData">The drag data.</param>
    /// <param name="allowedOps">Allowed drag operations.</param>
    /// <param name="x">X coordinate of the drag start.</param>
    /// <param name="y">Y coordinate of the drag start.</param>
    /// <returns>True if the drag operation is handled, false otherwise.</returns>
    bool StartDragging(CefBrowser browser, CefDragData dragData, CefDragOperationsMask allowedOps, int x, int y);

    /// <summary>
    /// Called to update the drag cursor during a drag-and-drop operation.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="operation">The current drag operation.</param>
    void UpdateDragCursor(CefBrowser browser, CefDragOperationsMask operation);

    /// <summary>
    /// Called when the scroll offset changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="x">New horizontal scroll offset.</param>
    /// <param name="y">New vertical scroll offset.</param>
    void OnScrollOffsetChanged(CefBrowser browser, double x, double y);

    /// <summary>
    /// Called when the IME composition range changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="selectedRange">The selected range.</param>
    /// <param name="characterBounds">Array of character bounds rectangles.</param>
    void OnImeCompositionRangeChanged(CefBrowser browser, CefRange selectedRange, CefRectangle[] characterBounds);

    /// <summary>
    /// Called when the text selection changes.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="selectedText">The selected text.</param>
    /// <param name="selectedRange">The selected range.</param>
    void OnTextSelectionChanged(CefBrowser browser, string selectedText, CefRange selectedRange);

    /// <summary>
    /// Called when a virtual keyboard is requested.
    /// </summary>
    /// <param name="browser">The browser instance.</param>
    /// <param name="inputMode">The requested text input mode.</param>
    void OnVirtualKeyboardRequested(CefBrowser browser, CefTextInputMode inputMode);
}
