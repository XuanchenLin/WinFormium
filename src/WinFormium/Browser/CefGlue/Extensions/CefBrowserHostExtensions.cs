// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

public static class CefBrowserHostExtensions
{
    public static unsafe bool SendDevToolsMessage(this CefBrowserHost browserHost, byte[] message)
    {
        fixed (byte* messagePtr = &message[0])
        {
            return browserHost.SendDevToolsMessage((IntPtr)messagePtr, message.Length);
        }
    }

    public static unsafe bool SendDevToolsMessage(this CefBrowserHost browserHost, ArraySegment<byte> message)
    {
        fixed (byte* messagePtr = &message.Array[message.Offset])
        {
            return browserHost.SendDevToolsMessage((IntPtr)messagePtr, message.Count);
        }
    }
}
