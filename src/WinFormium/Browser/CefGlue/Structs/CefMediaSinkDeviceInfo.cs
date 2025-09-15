// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// Device information for a MediaSink object.
/// </summary>
public readonly struct CefMediaSinkDeviceInfo
{
    private readonly string _ipAddress;
    private readonly int _port;
    private readonly string _modelName;

    public CefMediaSinkDeviceInfo(string ipAddress, int port, string modelName)
    {
        _ipAddress = ipAddress;
        _port = port;
        _modelName = modelName;
    }

    public readonly string IPAddress => _ipAddress;

    public readonly int Port => _port;

    public readonly string ModelName => _modelName;
}
