// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;

/// <summary>
/// Interface for handling print-related events and operations in the browser.
/// </summary>
public interface IPrintHandler
{
    /// <summary>
    /// Returns the PDF paper size in device units for the specified browser and DPI.
    /// </summary>
    /// <param name="browser">The browser instance requesting the paper size.</param>
    /// <param name="deviceUnitsPerInch">The device units per inch (DPI).</param>
    /// <returns>The paper size as a <see cref="CefSize"/> structure.</returns>
    CefSize GetPdfPaperSize(CefBrowser browser, int deviceUnitsPerInch);

    /// <summary>
    /// Called to display the print dialog.
    /// </summary>
    /// <param name="browser">The browser requesting the print dialog.</param>
    /// <param name="hasSelection">Indicates whether there is a selection to print.</param>
    /// <param name="callback">The callback to continue or cancel the print dialog.</param>
    /// <returns>True if the dialog is handled, false to use the default implementation.</returns>
    bool OnPrintDialog(CefBrowser browser, bool hasSelection, CefPrintDialogCallback callback);

    /// <summary>
    /// Called to initiate a print job.
    /// </summary>
    /// <param name="browser">The browser requesting the print job.</param>
    /// <param name="documentName">The name of the document to print.</param>
    /// <param name="pdfFilePath">The file path where the PDF should be saved.</param>
    /// <param name="callback">The callback to continue or cancel the print job.</param>
    /// <returns>True if the print job is handled, false to use the default implementation.</returns>
    bool OnPrintJob(CefBrowser browser, string documentName, string pdfFilePath, CefPrintJobCallback callback);

    /// <summary>
    /// Called to reset print-related state.
    /// </summary>
    /// <param name="browser">The browser for which to reset print state.</param>
    void OnPrintReset(CefBrowser browser);

    /// <summary>
    /// Called to update print settings before printing.
    /// </summary>
    /// <param name="browser">The browser requesting the print settings update.</param>
    /// <param name="settings">The print settings to update.</param>
    /// <param name="getDefaults">True if default settings should be retrieved.</param>
    void OnPrintSettings(CefBrowser browser, CefPrintSettings settings, bool getDefaults);

    /// <summary>
    /// Called when printing is about to start.
    /// </summary>
    /// <param name="browser">The browser that is starting to print.</param>
    void OnPrintStart(CefBrowser browser);
}
