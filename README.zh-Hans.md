<p align="center">
    <img src="./artworks/WinFormiumLogo.png" width="144" />
</p>
<h1 align="center">WinFormium 项目</h1>
<p align="center"><strong>用 HTML, CSS 和 JavaScript 轻松构建功能强大的 WinForm 应用程序。</strong></p>

# WinFormium

![GitHub](https://img.shields.io/github/license/XuanchenLin/WinFormium)
![Nuget](https://img.shields.io/nuget/v/WinFormium)
![Nuget](https://img.shields.io/nuget/dt/WinFormium)

[English](README.md) | 简体中文

## ⭐ 关于

WinFormium 是 .NET 平台上的一个开源框架，用于使用 HTML5、CSS3 和 JavaScript 创建 WinForm 应用程序的用户界面。 它基于 [Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home) 项目，该项目是 [Chromium Embedded Framework (CEF)](https://bitbucket.org/chromiumembedded/cef) 的 .NET 实现。

如果您正在寻找一个用于创建具有现代用户界面的 WinForm 应用程序的框架，WinFormium 是一个不错的选择。 您可以使用 HTML、CSS 和 JavaScript 创建用户界面，并使用 C# 编写应用程序的业务逻辑。

**如果您喜欢 👍，请给 WinFormium 项目一颗星 ⭐。**

如果这个项目对你有帮助，请考虑资助它。

[![支付宝](https://img.shields.io/badge/%E6%8D%90%E8%B5%A0-%E6%94%AF%E4%BB%98%E5%AE%9D-blue)](docs/assets/qrcode.png)
[![微信](https://img.shields.io/badge/%E6%8D%90%E8%B5%A0-%E5%BE%AE%E4%BF%A1-Green)](docs/assets/qrcode.png)

## 🖥️ 环境要求

**开发环境**

- .NET Framework 4.6.2 或更高版本 / .NET 6.0 或更高版本
- Visual Studio 2019 或更高版本（强烈建议使用 VS2022）

**部署环境**

- Microsoft Windows 7 Service Pack 1 或更高版本
- .Net Framework 4.6.2 或更高版本
- .NET 6.0 需要 Windows 7 Service Pack 1 或更高版本
- .NET 7.0/8.0 需要 Windows 10 或 Windows 11

这是一个 **仅限 Windows** 的框架，所以它目前不能在 Linux 或者 MacOS 环境运行。

支持的最低 Windows 版本是 Windows 7 Service Pack 1，并且 Windows 7 不支持某些功能（例如 DirectComposition 离屏渲染）。

## 🧰 入门

### 创建一个简单的应用程序

**1. 通过默认模板创建一个 WinForm 应用程序。**

**2. 安装 WinFormium NuGet 包**

打开 NuGet 包管理器来安装或使用 NuGet 包管理器控制台，然后运行以下命令来安装 WinFormium nuget 包：

```powershell
PM> Install-Package WinFormium
```

安装 WinFormium 所依赖的 Chromium Embedded Framework 依赖项：

```powershell
PM> Install-Package WinFormium.Runtime
```

CEF 运行库巨大，再加上众所周知的原因，中国内地玩家请自行设置 NuGet 使用国内镜像。

- **Azure CDN** - https://nuget.cdn.azure.cn/v3/index.json
- **华为云** - https://repo.huaweicloud.com/repository/nuget/v3/index.json

**3. 一个基本的 WinFormium 应用程序需要以下代码：**

按如下示例修改 **Program.cs** 文件中的代码：

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

创建一个类继承 **WinFormiumStartup** 来配置应用程序：

```csharp
using WinFormium;

class MyAPP : WinFormiumStartup
{
    protected override MainWindowCreationAction? UseMainWindow(MainWindowOptions opts)
    {
        // 设置应用程序的主窗体
        return opts.UseMainFormium<MyWindow>();
    }

    protected override void WinFormiumMain(string[] args)
    {
        // Main函数中的代码应该在这里，该函数只在主进程中运行。这样可以防止子进程运行一些不正确的初始化代码。
        ApplicationConfiguration.Initialize();
    }

    protected override void ConfigurationChromiumEmbedded(ChromiumEnvironmentBuiler cef)
    {
        // 在此处配置 Chromium Embedded Framwork
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // 在这里配置该应用程序的服务
    }
}
```

创建一个类实现 **Formium**，用于配置应用程序的主窗口：

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
        // 此处配置窗口的样式和属性，或留空以使用默认样式

        var style = builder.UseSystemForm();

        style.TitleBar = false;

        style.DefaultAppTitle = "My first WinFomrim app";

        return style;
    }
}
```

**4. 生成并运行你的第一个 WinFormium 应用程序**

## 📖 文档

有关更多信息，请参阅 - [文档](docs/en/Documentation.md) 或 [Wiki](https://github.com/XuanchenLin/WinFormium/wiki)。

## 🤖 示例代码

- [Minimal WinFormium App](./examples/MinimalWinFormiumApp) - 介绍 WinFormium 的基本用法。

## 🔗 第三方库引用和工具集

- CEF - [https://bitbucket.org/chromiumembedded/cef]()
- Xilium.CefGlue - [https://gitlab.com/xiliumhq/chromiumembedded/cefglue]()
- Vanara.Library - [https://github.com/dahall/Vanara/]()
- Vortice.Windows - [https://github.com/amerkoleci/Vortice.Windows]()
- SkiaSharp - [https://github.com/mono/SkiaSharp]()
- React - [https://github.com/facebook/react]()
- React-Router - [https://github.com/remix-run/react-router]()
- Vite - [https://github.com/vitejs/vite]()

## 🏆 灵感来源

在创建此版本的 WinFormium 时，我受到以下歌曲和专辑的启发，特此感谢。

- **Strandels** - Chance Of Rain
- **One Direction** - What a Feeling (Made In The A.M.)
- **Thomas Rhett** - VHS (Center Point Road)
- **Sammy Kershaw** - She Don't Know She's Beautiful (Haunted Heart)
- **Chrissy Steele** - Two Bodies (Magnet To Steele)
- **Halestorm** - I Like It Heavy (Into the Wild Life)
- **Joan Jett & The Blackhearts** - I Hate Myself for Loving You (Up Your Alley)
