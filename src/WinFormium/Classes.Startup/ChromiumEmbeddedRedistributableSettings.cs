// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Provides settings and logic for managing Chromium Embedded Framework (CEF) redistributable files,
/// including auto-detection, validation, and runtime configuration.
/// </summary>
public sealed class ChromiumEmbeddedRedistributableSettings
{
    private readonly string chromiumVersion;

    const string RESOURCE_DIR = "Resources";
    const string LOCALES_DIR = "locales";
    const string UP_LEVEL_DIR = "..";

    const string ANY_CPU_RUNTIME_DIR = "runtimes";
    const string DEFAULT_CEF_DIR = "fx";

    /// <summary>
    /// Gets the version of the Chromium runtime.
    /// </summary>
    public string ChromiumRuntimeVersion => chromiumVersion;

    /// <summary>
    /// Gets the common CEF runtime directory path for the current architecture and Chromium version.
    /// </summary>
    internal string CommonCefRuntimeDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @$"{WinFormiumApp.WINFROMIUM_NAME}", chromiumVersion);

    /// <summary>
    /// Gets the base directory where the application is running.
    /// </summary>
    public string ApplicationRunningDirectory => System.AppContext.BaseDirectory;

    /// <summary>
    /// Gets or sets the directory path where libcef.dll is located.
    /// </summary>
    public string LibCefDirPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the directory path where CEF resource files are located.
    /// </summary>
    public string ResourceDirPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the directory path where CEF locale files are located.
    /// </summary>
    public string LocaleDirPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception if the CEF redistributable is not valid.
    /// </summary>
    public bool ThrowExceptionIfCefRedistributableIsNotValid { get; set; } = true;

    /// <summary>
    /// Gets the current platform architecture.
    /// </summary>
    public PlatformArchitecture CurrentArchitecture { get; }

    /// <summary>
    /// Gets a value indicating whether the CEF redistributable is valid (libcef.dll and resources exist).
    /// </summary>
    public bool IsCefRedistributableValid => EnsureLibCefExists(LibCefDirPath) && EnsureLibCefResourceDirExists(ResourceDirPath);

    /// <summary>
    /// Gets or sets a value indicating whether to use the internal runtime downloader.
    /// </summary>
    public bool UseInternalRuntimeDownloader { get; set; } = true;

    /// <summary>
    /// Gets or sets the default download URL template for the CEF runtime.
    /// </summary>
    public string DefaultDownloadUrl { get; set; } = "https://gitcode.com/linxuanchen/WinFormium-Runtime-Releases/releases/download/v{{VERSION}}/WinFormium_Runtime_{{ARCH}}.exe";

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromiumEmbeddedRedistributableSettings"/> class.
    /// </summary>
    /// <param name="architecture">The platform architecture.</param>
    /// <param name="chromiumVersion">The Chromium version.</param>
    internal ChromiumEmbeddedRedistributableSettings(PlatformArchitecture architecture, string chromiumVersion)
    {
        CurrentArchitecture = architecture;
        this.chromiumVersion = chromiumVersion;

        AutoDetectCefRedistributable();

    }

    /// <summary>
    /// Checks if the specified directory contains a valid libcef.dll file.
    /// </summary>
    /// <param name="path">The directory path to check.</param>
    /// <returns>True if libcef.dll exists in the directory; otherwise, false.</returns>
    private static bool EnsureLibCefExists(string? path) => path != null && File.Exists(Path.Combine(path, "libcef.dll"));

    /// <summary>
    /// Checks if the specified directory contains valid CEF resource files and locales.
    /// </summary>
    /// <param name="path">The resource directory path to check.</param>
    /// <returns>True if resource files and locales exist; otherwise, false.</returns>
    private static bool EnsureLibCefResourceDirExists(string? path) => path != null && Directory.Exists(path) && Directory.GetFiles(path, "*.pak", SearchOption.TopDirectoryOnly).Length > 0 && Directory.Exists(Path.Combine(path, "locales")) && Directory.GetFiles(Path.Combine(path, "locales"), "*.pak", SearchOption.TopDirectoryOnly).Length > 0;

    /// <summary>
    /// Automatically detects and sets the paths for the CEF redistributable files and resources.
    /// </summary>
    internal void AutoDetectCefRedistributable()
    {
        var args = Environment.GetCommandLineArgs();

        var libCefPathArg = args?.FirstOrDefault(x => x.StartsWith("--libcef-dir-path"))?.Split('=');

        string[] searchPaths = [

            Path.Combine(CommonCefRuntimeDirectory, CurrentArchitecture.ToString()),
            ApplicationRunningDirectory,
            Path.Combine(ApplicationRunningDirectory, CurrentArchitecture.ToString()),
            Path.Combine(ApplicationRunningDirectory, DEFAULT_CEF_DIR, CurrentArchitecture.ToString()),
            Path.Combine(ApplicationRunningDirectory, ANY_CPU_RUNTIME_DIR, $"win-{CurrentArchitecture}", "native"),
        ];

        if (libCefPathArg != null && libCefPathArg.Length == 2 && EnsureLibCefExists(libCefPathArg[1]))
        {
            LibCefDirPath = libCefPathArg[1];
        }
        else
        {


            foreach (var path in searchPaths)
            {
                if (EnsureLibCefExists(path))
                {
                    LibCefDirPath = path;
                    break;
                }
            }
        }

        if (string.IsNullOrEmpty(LibCefDirPath)) return;

        searchPaths =
        [
            LibCefDirPath,
            Path.GetFullPath(Path.Combine(LibCefDirPath, UP_LEVEL_DIR)),
            Path.GetFullPath(Path.Combine(LibCefDirPath, UP_LEVEL_DIR, RESOURCE_DIR)),
            Path.Combine(LibCefDirPath, RESOURCE_DIR)
        ];

        foreach (var path in searchPaths)
        {
            if (EnsureLibCefResourceDirExists(path))
            {
                ResourceDirPath = path;
                LocaleDirPath = Path.Combine(ResourceDirPath, LOCALES_DIR);
                break;
            }
        }

    }
}
