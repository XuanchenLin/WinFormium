// THIS FILE IS PART OF NanUI PROJECT
// THE NanUI PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.WebResource;



/// <summary>
/// Provides options for mapping embedded files as web resources.
/// </summary>
public sealed class EmbeddedFileMappingOptions : WebResourceOptions
{
    /// <summary>
    /// Gets or initializes the directory name within the embedded resources to be mapped.
    /// </summary>
    public string? EmbeddedResourceDirectoryName { get; init; }

    /// <summary>
    /// Gets or initializes the default namespace used to locate embedded resources.
    /// </summary>
    public string? DefaultNamespace { get; init; }

    /// <summary>
    /// Gets or initializes the assembly that contains the embedded resources.
    /// </summary>
    public required Assembly ResourceAssembly { get; init; }
}
