// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Implement this interface to receive notification when tracing has completed.
/// The methods of this class will be called on the browser process UI thread.
/// </summary>
public abstract unsafe partial class CefEndTracingCallback
{
    private void on_end_tracing_complete(cef_end_tracing_callback_t* self, cef_string_t* tracing_file)
    {
        CheckSelf(self);

        OnEndTracingComplete(cef_string_t.ToString(tracing_file));
    }
    
    /// <summary>
    /// Called after all processes have sent their trace data. |tracing_file| is
    /// the path at which tracing data was written. The client is responsible for
    /// deleting |tracing_file|.
    /// </summary>
    protected abstract void OnEndTracingComplete(string tracingFile);
}
