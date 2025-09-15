// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewDragHandler : CefDragHandler
{
    public IDragHandler Handler { get; }

    public WebViewDragHandler(IDragHandler handler)
    {
        Handler = handler;
    }

    protected override bool OnDragEnter(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask)
    {
        return Handler.OnDragEnter(browser, dragData, mask);
    }

    protected override void OnDraggableRegionsChanged(CefBrowser browser, CefFrame frame, CefDraggableRegion[] regions)
    {
        Handler.OnDraggableRegionsChanged(browser, frame, regions);
    }

}
