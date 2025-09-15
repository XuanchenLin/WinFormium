// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
public sealed class CefDraggableRegion
{
    internal static unsafe CefDraggableRegion FromNative(cef_draggable_region_t* ptr)
    {
        return new CefDraggableRegion(ptr);
    }

    private readonly CefRectangle _bounds;
    private readonly bool _draggable;

    private unsafe CefDraggableRegion(cef_draggable_region_t* ptr)
    {
        _bounds = new CefRectangle(
            ptr->bounds.x,
            ptr->bounds.y,
            ptr->bounds.width,
            ptr->bounds.height
            );
        _draggable = ptr->draggable != 0;
    }

    public CefRectangle Bounds { get { return _bounds; } }

    public bool Draggable { get { return _draggable; } }
}
