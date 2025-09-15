// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;

/// <summary>
/// Specifies the HTTP methods that can be used for web resource requests.
/// </summary>
public enum WebResourceRequestMethod
{
    /// <summary>
    /// Method is not indicated.
    /// </summary>
    All,

    /// <summary>
    /// The get method.
    /// </summary>
    GET,

    /// <summary>
    /// The post method.
    /// </summary>
    POST,

    /// <summary>
    /// The put method.
    /// </summary>
    PUT,

    /// <summary>
    /// The delete method.
    /// </summary>
    DELETE,

    /// <summary>
    /// The head method.
    /// </summary>
    HEAD,

    /// <summary>
    /// The options method.
    /// </summary>
    OPTIONS,

    /// <summary>
    /// The patch method.
    /// </summary>
    PATCH

}
