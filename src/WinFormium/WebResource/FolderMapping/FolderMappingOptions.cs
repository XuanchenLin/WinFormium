// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.WebResource;
/// <summary>
/// Represents the options for mapping a local folder as a web resource.
/// </summary>
public class FolderMappingOptions : WebResourceOptions
{
    /// <summary>
    /// Gets the path of the local folder to be mapped as a web resource.
    /// </summary>
    public required string FolderPath { get; init; }
}
