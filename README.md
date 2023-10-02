<p align="center">
    <img src="./artworks/WinFormiumLogo.png" width="144" />
</p>
<h1 align="center">The WinFormium Project</h1>
<p align="center"><strong>Easily buid powerful WinForm applications with HTML, CSS and JavaScript.</strong></p>

# WinFormium

![GitHub](https://img.shields.io/github/license/XuanchenLin/WinFormium)
![Nuget](https://img.shields.io/nuget/v/WinFormium)
![Nuget](https://img.shields.io/nuget/dt/WinFormium)

English | [简体中文](README.zh-Hans.md)

## ⭐ About

WinFormium is a open source framework on .NET platform for creating user interface of WinForm Applicaitons using HTML5, CSS3, and JavaScript. It is based on the [Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home) project, which is a .NET wrapper around the [Chromium Embedded Framework](https://bitbucket.org/chromiumembedded/cef).

If you are looking for a framework for creating a WinForm application with a modern user interface, WinFormium is a good choice. you can use HTML, CSS, and JavaScript to create a user interface, and use C# to write the business logic of the application.

**Please give WinFormium project a star⭐ if you like it.**

If this project helps, please consider funding it.

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/mrjson?country.x=C2&locale.x=zh_XC)

## 🖥️ Requirements

**For Development**

- .NET Framework 4.6.2 or higher / .NET 6.0 or higher
- Visual Studio 2019 or higher (VS2022 is recommended)

**For Deployment**

- Microsoft Windows 7 Service Pack 1 or higher
- .Net Framework 4.6.2 or higher
- .NET 6.0 for Windows 7 and higher.
- .NET 7.0/8.0 for Windows 10 and higher.

This is a **Windows Only** framework, it can not run on Linux or Mac OS.

The minimum supported Windows is Windows 7 Service Pack 1, and some features (such as DirectComposition Offscreen Rendering) are not supported on Windows 7.

## 🧰 Getting Started

### Create a simple Application

**1. Create a WinForm Application by default template.**

**2. Install WinFormium NuGet Package**

Open the NuGet Package Manager to install or use NuGet Package Manager Console, and run the following command to install WinFormium nuget package:

```powershell
PM> Install-Package WinFormium
```

Install the dependencies of Chromium Embedded Framework that WinFormium depends on:

```powershell
PM> Install-Package WinFormium.Runtime
```

**3. A basic WinFormium application requires the following code:**

Modify the code in the **Program.cs** file as follows:

```csharp
using WinFormium;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var builder = WinFormiumApp.CreateBuilder();

        builder.UseWinFormiumApp<MyApp>();

        var app = builder.Build();

        app.Run();
    }
}
```

Create a class implements **WinFormiumStartup** for configuring the application:

```csharp
using WinFormium;

class MyAPP : WinFormiumStartup
{
    protected override MainWindowCreationAction? UseMainWindow(MainWindowOptions opts)
    {
        // Configure the main window of this application
        return opts.UseMainFormium<MyWindow>();
    }

    protected override void WinFormiumMain(string[] args)
    {
        // The codes in Main function should be here, this function only runs in Main process. So it can prevent the codes in Main process running in sub-processes.
        ApplicationConfiguration.Initialize();
    }

    protected override void ConfigurationChromiumEmbedded(ChromiumEnvironmentBuiler cef)
    {
        // Configure the Chromium Embedded Framework here
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // Configure the services of this application here
    }
}
```

Create a class implements **Formium** for configuring the main window of the application:

```csharp
using WinFormium;
using WinFormium.Forms;

class MyWindow : Formium
{
    public MyWindow()
    {
        Url = "https://www.google.com";
    }

    protected override FormStyle ConfigureWindowStyle(WindowStyleBuilder builder)
    {
        // Configure the style of the window here or leave it blank to use the default style

        var style = builder.UseSystemForm();

        style.TitleBar = false;

        style.DefaultAppTitle = "My first WinFomrim app";

        return style;
    }
}
```

**4. Build and run**

## 📖 Documentation

For more info please see - [Documentation](docs/en/Documentation.md) or [Wiki](https://github.com/XuanchenLin/WinFormium/wiki)

## 🤖 Demos

- [Minimal WinFormium App](./examples/MinimalWinFormiumApp) - Introduction to the basic usage of WinFormium.

## 🔗 Third-Party References & Tools

- CEF - [https://bitbucket.org/chromiumembedded/cef]()
- Xilium.CefGlue - [https://gitlab.com/xiliumhq/chromiumembedded/cefglue/]()
- Vanara.Library - [https://github.com/dahall/Vanara/]()
- Vortice.Windows - [https://github.com/amerkoleci/Vortice.Windows]()
- SkiaSharp - [https://github.com/mono/SkiaSharp]()
- React - [https://github.com/facebook/react]()
- React-Router - [https://github.com/remix-run/react-router]()
- Vite - [https://github.com/vitejs/vite]()

## 🏆 Inspirations

I was inspired by the following songs and albums when creating this version of WinFormium.

- **Strandels** - Chance Of Rain
- **One Direction** - What a Feeling (Made In The A.M.)
- **Thomas Rhett** - VHS (Center Point Road)
- **Sammy Kershaw** - She Don't Know She's Beautiful (Haunted Heart)
- **Chrissy Steele** - Two Bodies (Magnet To Steele)
- **Halestorm** - I Like It Heavy (Into the Wild Life)
- **Joan Jett & The Blackhearts** - I Hate Myself for Loving You (Up Your Alley)
