// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

class PromiseData : IDisposable
{
    public required string CallId { get; init; }
    public required CefV8Context V8Context { get; init; }
    public required CefV8Value Promise { get; init; }


    public void Dispose()
    {
        Promise.Dispose();
    }
}

