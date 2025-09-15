// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;
partial class WebViewCore
{
    public bool MainFrameChanging { get; private set; } = false;


    public void Close() 
    {
        BrowserHost?.CloseBrowser(false);
    }

    public void CloseImmediately()
    {
        BrowserHost?.CloseBrowser(true);
    }

}
