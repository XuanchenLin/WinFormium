// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;
class WebViewPrintHandler : CefPrintHandler
{
    public IPrintHandler Handler { get; }

    public WebViewPrintHandler(IPrintHandler handler)
    {
        Handler = handler;
    }

    protected override CefSize GetPdfPaperSize(CefBrowser browser, int deviceUnitsPerInch)
    {
        return Handler.GetPdfPaperSize(browser, deviceUnitsPerInch);
    }

    protected override bool OnPrintDialog(CefBrowser browser, bool hasSelection, CefPrintDialogCallback callback)
    {
        return Handler.OnPrintDialog(browser, hasSelection, callback);
    }

    protected override bool OnPrintJob(CefBrowser browser, string documentName, string pdfFilePath, CefPrintJobCallback callback)
    {
        return Handler.OnPrintJob(browser, documentName, pdfFilePath, callback);
    }

    protected override void OnPrintReset(CefBrowser browser)
    {
        Handler.OnPrintReset(browser);
    }

    protected override void OnPrintSettings(CefBrowser browser, CefPrintSettings settings, bool getDefaults)
    {
        Handler.OnPrintSettings(browser, settings, getDefaults);
    }

    protected override void OnPrintStart(CefBrowser browser)
    {
        Handler.OnPrintStart(browser);
    }
}
