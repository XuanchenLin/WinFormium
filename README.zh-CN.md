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

`WinFormium` 是一个开源 .NET 库，它基于 [Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home) 项目，该项目是 [Chromium 嵌入式框架](https://bitbucket.org/chromiumembedded/cef) 的 .NET 包装器。它允许 .NET 开发者使用 HTML、CSS 和 JavaScript 创建现代化、视觉上引人入胜的 WinForm 应用程序。与 WPF 相比，您可以轻松便捷地实现丰富且高度交互的用户界面，而无需学习复杂的 XAML 语言。它是 [NanUI](https://github.com/NetDimension/NanUI) 的一个分支，但不同之处在于它可以使用 AOT 技术将项目直接编译为原生代码，从而减少软件分发包的大小，并有效防止代码被反编译。

目前，WinFormium 项目仍处于早期开发阶段，因此暂时不会提供源代码。本仓库将不定期发布示例程序和代码，我们期待 WinFormium 早日问世。

**如果您喜欢 WinFormium 项目，请给它一个 star⭐。**

如果此项目对您有所帮助，请考虑资助。

[![支付宝](https://img.shields.io/badge/%E6%8D%90%E8%B5%A0-%E6%94%AF%E4%BB%98%E5%AE%9D-blue)](docs/assets/qrcode.png)
[![微信](https://img.shields.io/badge/%E6%8D%90%E8%B5%A0-%E5%BE%AE%E4%BF%A1-Green)](docs/assets/qrcode.png)

## 🖥️ 环境要求

**开发环境**

- .NET 8.0/9.0，兼容 AOT
- Visual Studio 2022 17.13

**部署环境**

- Microsoft Windows 7 Service Pack 1 或更高版本
- .NET 8.0/9.0（以独立版本或 AOT 版本发布）

这是一个 **仅限 Windows** 的框架，所以它目前不能在 Linux 或者 MacOS 环境运行。

支持的最低 Windows 版本是 Windows 7 Service Pack 1，并且 Windows 7 不支持某些功能（例如 DirectComposition 离屏渲染）。
这是一个**仅限 Windows** 的框架，无法在 Linux 或 Mac OS 上运行。
