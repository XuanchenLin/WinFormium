// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.FileProviders;

using BlazorHybridExampleApp.Components;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

using WinFormium;
using WinFormium.BlazorHybrid;

namespace BlazorHybridExampleApp;
internal class MainWindow : BlazorFormium
{
    BlazorFormiumOptions? _opts;

    public MainWindow()
    {
        Url = "https://blazorapp.local/";

        WindowTitle = "Blazor Hybrid Example App";

        Icon = new System.Drawing.Icon(new MemoryStream(Properties.Resources.WinFormiumBlazor));


        BackColor = System.Drawing.Color.Transparent;
    }

    public override BlazorFormiumOptions Options
    {
        get
        {
            if (_opts == null)
            {
                var opts = new BlazorFormiumOptions()
                {
                    Scheme = "https",
                    DomainName = "blazorapp.local",
                    ConfigureServices = services =>
                    {
                        services.AddFluentUIComponents(options =>
                        {
                            options.ValidateClassNames = false;

                        });

                        services.AddHttpClient();
                    },
                };

                opts.RootComponents.Add<App>("#app");
                opts.RootComponents.Add<HeadOutlet>("head::after");
                opts.StaticResources.Add(new BlazorFormiumAssemblyResources(typeof(MainWindow).Assembly));

                _opts = opts;
            }

            return _opts;
        }
    }



    protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
    {
        var settings = opts.UseDefaultWindow();

        settings.ExtendsContentIntoTitleBar = true;


        //settings.SystemBackdropType = SystemBackdropType.Transient;


        return settings;
    }


}