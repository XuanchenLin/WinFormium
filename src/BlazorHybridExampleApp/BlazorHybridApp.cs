// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using WinFormium;

namespace BlazorHybridExampleApp;

internal class BlazorHybridApp : AppStartup
{
    protected override AppCreationAction? OnApplicationStartup(StartupSettings settings)
    {
        return settings.UseMainWindow(new MainWindow());
    }


    protected override void ConfigureSubprocessAppSettings(SubprocessAppSettings subprocessAppSettings)
    {
#if !DEBUG
        if (File.Exists("BrowserSubprocess.exe"))
        {
            subprocessAppSettings.SubprocessFilePath = "BrowserSubprocess.exe";
        }
#endif
    }
}