// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;


/// <summary>
/// Provides settings for configuring the subprocess application used by WinFormium.
/// </summary>
public sealed class SubprocessAppSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the subprocess path has been explicitly set.
    /// </summary>
    internal bool SubprocessPathIsSet { get; set; } = false;

    string _subprocessPath = string.Empty;

    /// <summary>
    /// Gets the platform architecture for the subprocess application.
    /// </summary>
    public PlatformArchitecture Artchitecture { get; }

    /// <summary>
    /// Gets or sets the file path of the subprocess application.
    /// </summary>
    public string SubprocessFilePath
    {
        get => _subprocessPath;
        set
        {
            if (value != _subprocessPath)
            {
                _subprocessPath = value;
                SubprocessPathIsSet = true;
            }

        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception if the subprocess file does not exist.
    /// </summary>
    public bool ThrowExceptionIfSubprocessIsNotExists { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubprocessAppSettings"/> class with the specified platform architecture.
    /// </summary>
    /// <param name="artchitecture">The platform architecture for the subprocess application.</param>
    internal SubprocessAppSettings(PlatformArchitecture artchitecture)
    {
        Artchitecture = artchitecture;
    }

}
