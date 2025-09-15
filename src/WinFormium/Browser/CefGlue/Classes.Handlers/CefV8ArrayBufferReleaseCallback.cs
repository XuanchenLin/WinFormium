// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.Browser.CefGlue.Interop;

namespace WinFormium.Browser.CefGlue;
/// <summary>
/// Callback interface that is passed to CefV8Value::CreateArrayBuffer.
/// </summary>
public abstract unsafe partial class CefV8ArrayBufferReleaseCallback
{
    private void release_buffer(cef_v8array_buffer_release_callback_t* self, void* buffer)
    {
        CheckSelf(self);

        ReleaseBuffer((IntPtr)buffer);
    }

    /// <summary>
    /// Called to release |buffer| when the ArrayBuffer JS object is garbage
    /// collected. |buffer| is the value that was passed to CreateArrayBuffer
    /// along with this object.
    /// </summary>
    protected abstract void ReleaseBuffer(IntPtr buffer);
}
