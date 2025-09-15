// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewDialogHandler : CefDialogHandler
{
    public IDialogHandler Handler { get; }

    public WebViewDialogHandler(IDialogHandler handler)
    {
        Handler = handler;
    }

    protected override bool OnFileDialog(CefBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, string[] acceptFilters, CefFileDialogCallback callback)
    {
        return Handler.OnFileDialog(browser, mode, title, defaultFilePath, acceptFilters, callback);
    }
}
