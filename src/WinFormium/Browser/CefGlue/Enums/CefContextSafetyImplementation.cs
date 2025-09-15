// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.CefGlue;

/// <summary>
/// CEF offers two context safety implementations with different performance characteristics.
/// </summary>
public enum CefContextSafetyImplementation : int
{
    /// <summary>
    /// The default implementation (value of 0) uses a map of hash values and should provide
    /// better performance in situations with a small number contexts.
    /// </summary>
    SafeDefault = 0,

    /// <summary>
    /// The alternate implementation (value of 1) uses a hidden value attached to each context
    /// and should provide better performance in situations with a large number of contexts.
    /// </summary>
    SafeAlternate = 1,

    /// <summary>
    /// If you need better performance in the creation of V8 references and you
    /// plan to manually track context lifespan you can disable context safety by
    /// specifying a value of -1.
    /// </summary>
    Disabled = -1,
}
