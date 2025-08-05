// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WinFormium;

namespace MinimalExampleApp;
internal class StartupWindow : Formium
{
    public StartupWindow() 
    {
        StartPosition = FormStartPosition.CenterScreen;
        Icon = new Icon(new MemoryStream(Properties.Resources.ColorsIcon));
        BackColor = Color.Transparent;
        Maximizable = false;
        TopMost = true;
        //Minimizable = false;
        
        Size = new Size(620, 620);
        Url = "https://localresources.app/startup/";
    }

    protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
    {
        var settings = opts.UseLayeredWindow();
        settings.Resizable = false;
        settings.DisableAppRegionMenu = true;
        return settings;
        //var settings = opts.UseDefaultWindow();
        //settings.Resizable = false;
        //settings.ShowWindowDecorators = false;
        //settings.ExtendsContentIntoTitleBar = true;
        //settings.SystemBackdropType = SystemBackdropType.None;

        //return settings;
    }
}
