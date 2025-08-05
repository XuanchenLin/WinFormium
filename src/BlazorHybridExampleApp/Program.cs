// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium;
using WinFormium.BlazorHybrid;

namespace BlazorHybridExampleApp;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var builder = WinFormiumApp.CreateBuilder();

        var app = builder
            .UseDevTools()
            .UseWinFormiumApp<BlazorHybridApp>()
            .Build();

        app.Run();
    }
}