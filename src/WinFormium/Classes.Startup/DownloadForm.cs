// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Diagnostics;

namespace WinFormium;
internal partial class DownloadForm : Form
{

    public PlatformArchitecture Architecture => RedistSettings.CurrentArchitecture;

    public DownloadForm(ChromiumEmbeddedRedistributableSettings redistSettings)
    {
        InitializeComponent();

        RedistSettings = redistSettings;

        Shown += DownloadForm_Shown;
    }

    private void DownloadForm_Shown(object? sender, EventArgs e)
    {
        DoDownloadAndInstall();
    }

    private async void DoDownloadAndInstall()
    {
        var packageFileName = $"WinFormium_Runtime_{RedistSettings.CurrentArchitecture}-{RedistSettings.ChromiumRuntimeVersion}.exe";
        var destinationFilePath = Path.Combine(Path.GetTempPath(), packageFileName);


        try
        {
            lblInfo.Text = "开始下载 WinFormium 运行时，请稍等...";
            var downloadUrl = GetDownloadUrl();


            using var client = new HttpClient();
            using var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);

            using var contentStream = await response.Content.ReadAsStreamAsync();

            // 获取文件总大小
            var totalBytes = response.Content.Headers.ContentLength;

            // 创建目标文件流
            using var destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None);

            byte[] buffer = new byte[4096];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                // 写入目标文件
                await destinationStream.WriteAsync(buffer, 0, bytesRead);

                // 更新进度条
                totalBytesRead += bytesRead;
                if (totalBytes.HasValue)
                {
                    int progressPercentage = (int)((totalBytesRead * 100) / totalBytes.Value);
                    MainProgressBar.Value = progressPercentage;

                    lblInfo.Text = string.Format("正在下载 WinFormium 运行时 {0}% ...", progressPercentage);

                }
            }

            destinationStream.Close();

            lblInfo.Text = $"下载失败完成，准备解压缩...";
        }
        catch (Exception ex)
        {
            lblInfo.Text = $"下载失败：{ex.Message}";
            btnCancel.Visible = true;
            return;
        }

        try
        {
            lblInfo.Text = "正在解压 WinFormium 运行时，请稍等...";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = destinationFilePath,
                    Verb = "runas",
                    UseShellExecute = true,
                }
            };
            process.Start();
            process.WaitForExit();
            lblInfo.Text = "解压完成！";

            await Task.Delay(2000);

            // 删除临时文件
            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }

            // 关闭窗口
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Win32Exception)
        {
            lblInfo.Text = $"解压失败，包格式不正确或用户取消了解压操作。";
            btnCancel.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblInfo.Text = $"安装失败：{ex.Message}";
            btnCancel.Visible = true;
            return;
        }



    }

    private string GetDownloadUrl()
    {
        var url = RedistSettings.DefaultDownloadUrl;

        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("The download URL is not set.");
        }

        if (url.Contains("{{VERSION}}") == false)
        {
            throw new Exception("The download URL must contain {{VERSION}}.");
        }

        if (url.Contains("{{ARCH}}") == false)
        {
            throw new Exception("The download URL must contain {{ARCH}}.");
        }

        url = url.Replace("{{VERSION}}", RedistSettings.ChromiumRuntimeVersion);
        url = url.Replace("{{ARCH}}", RedistSettings.CurrentArchitecture.ToString());
        return url;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    public ChromiumEmbeddedRedistributableSettings RedistSettings { get; }
}
