// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

partial class DownloadForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        MainProgressBar = new ProgressBar();
        lblInfo = new Label();
        btnCancel = new Button();
        SuspendLayout();
        // 
        // MainProgressBar
        // 
        MainProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        MainProgressBar.Location = new Point(19, 101);
        MainProgressBar.Margin = new Padding(2, 2, 2, 2);
        MainProgressBar.Name = "MainProgressBar";
        MainProgressBar.Size = new Size(464, 28);
        MainProgressBar.TabIndex = 0;
        // 
        // lblInfo
        // 
        lblInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblInfo.BackColor = Color.Transparent;
        lblInfo.Location = new Point(19, 17);
        lblInfo.Margin = new Padding(2, 0, 2, 17);
        lblInfo.Name = "lblInfo";
        lblInfo.Size = new Size(464, 65);
        lblInfo.TabIndex = 1;
        lblInfo.TextAlign = ContentAlignment.BottomLeft;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(366, 161);
        btnCancel.Margin = new Padding(2, 2, 2, 2);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(117, 33);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "关闭(&C)";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Visible = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // DownloadForm
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        BackColor = Color.White;
        ClientSize = new Size(502, 213);
        ControlBox = false;
        Controls.Add(btnCancel);
        Controls.Add(lblInfo);
        Controls.Add(MainProgressBar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Margin = new Padding(2, 2, 2, 2);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "DownloadForm";
        Padding = new Padding(17, 17, 17, 17);
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "下载 WinFormium 运行时";
        ResumeLayout(false);
    }

    #endregion

    private ProgressBar MainProgressBar;
    private Label lblInfo;
    private Button btnCancel;
}