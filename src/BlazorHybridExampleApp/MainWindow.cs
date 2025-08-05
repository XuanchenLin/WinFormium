// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.FileProviders;

using WinFormium;
using WinFormium.BlazorHybrid;

namespace BlazorHybridExampleApp;
internal class MainWindow : BlazorFormium
{
    public MainWindow()
    {
        Url = "https://blazorapp.local/";

        WindowTitle = "Blazor Hybrid Example App";

        Icon = new Icon(new MemoryStream(Properties.Resources.WinFormiumBlazor));

        StaticResources = typeof(MainWindow).Assembly;

        BackColor = Color.Transparent;
    }

    public override Type RootComponent => typeof(Counter);
    public override string BlazorServerAddress => "https://blazorapp.local/";

    protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
    {
        var settings = opts.UseDefaultWindow();

        //settings.SystemBackdropType = SystemBackdropType.Transient;


        return settings;
    }


}