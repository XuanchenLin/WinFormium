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

The `WinFormium` is an open-source .NET library which is based on the [Xilium.CefGlue](https://bitbucket.org/xilium/xilium.cefglue/wiki/Home) project a .NET wrapper around the [Chromium Embedded Framework](https://bitbucket.org/chromiumembedded/cef). It allows .NET developers to create modern, visually appealing WinForm applications using HTML, CSS and JavaScript. Compared to WPF, you can easily and simply implement rich and highly interactive user interfaces without having to learn the complex XAML language. It is an offshoot of [NanUI](https://github.com/NetDimension/NanUI), but the difference is that it can use AOT technology to compile the project directly into native code, thereby reducing the size of software distribution and effectively protecting your code from being decompiled.

Currently, the WinFormium project is still in early development, so the source code will not be provided for the time being. This repository will release sample programs and codes from time to time, and we look forward to seeing WinFormium soon.

**Please give WinFormium project a star⭐ if you like it.**

If this project helps, please consider funding it.

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/mrjson?country.x=C2&locale.x=zh_XC)

## 🖥️ Requirements

**For Development**

- .NET 8.0/9.0 with AOT Compatible
- Visual Studio 2022 17.13

**For Deployment**

- Microsoft Windows 7 Service Pack 1 or higher
- .NET 8.0/9.0 (publish as self-contained or AOT)

This is a **Windows Only** framework, it can not run on Linux or Mac OS.

The minimum supported Windows is Windows 7 Service Pack 1, and some features (such as transparent renderings) are not supported on Windows 7.

