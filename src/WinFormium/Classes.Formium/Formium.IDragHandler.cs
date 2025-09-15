// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
public partial class Formium : IDragHandler
{
    protected static readonly object objLock = new object();




    bool IDragHandler.OnDragEnter(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask)
    {

        if (!AllowDrop) return false;

        var args = new DragEnterEventArgs(browser, dragData, mask);

        InvokeIfRequired(() =>
        {
            DragEnter?.Invoke(this, args);
        });

        return args.AllowDragEnter || (DragHandler?.OnDragEnter(browser, dragData, mask) ?? false);
    }

    void IDragHandler.OnDraggableRegionsChanged(CefBrowser browser, CefFrame frame, CefDraggableRegion[] regions)
    {
        DragHandler?.OnDraggableRegionsChanged(browser, frame, regions);

        if (browser.IsPopup) return;

        lock (objLock)
        {
            IsDraggableRegion = (point) => {
                


                var hitNoDrag = regions.Any(r => !r.Draggable && ContainsPoint(r, point));

                if (hitNoDrag) return false;

                return regions.Any(r => r.Draggable && ContainsPoint(r, point));
            };
        }


    }


    private bool ContainsPoint(CefDraggableRegion region, Point point)
    {
        var width = region.Bounds.Width;
        var height = region.Bounds.Height;
        var x = region.Bounds.X;
        var y = region.Bounds.Y;


        width = (int)(width * DpiScaleFactor);
        height = (int)(height * DpiScaleFactor);
        x = (int)(x * DpiScaleFactor);
        y = (int)(y * DpiScaleFactor);


        return point.X >= x && point.X <= (x + width)
            && point.Y >= y && point.Y <= (y + height);
    }

    private Func<Point, bool>? IsDraggableRegion;

    /// <summary>
    /// Occurs when an object is dragged into the form's bounds.
    /// </summary>
    public event EventHandler<DragEnterEventArgs>? DragEnter;
}
