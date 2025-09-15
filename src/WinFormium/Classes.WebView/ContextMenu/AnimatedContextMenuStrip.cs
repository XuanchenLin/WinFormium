// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing.Imaging;

namespace WinFormium.ContextMenu;

#region WinFormDesignerDisabler
partial class _WinFormDesignerDisabler { }
#endregion

enum CefMenuIdentifiers
{
    NOT_FOUND = -1,

    // Navigation.
    MENU_ID_BACK = 100,
    MENU_ID_FORWARD = 101,
    MENU_ID_RELOAD = 102,
    MENU_ID_RELOAD_NOCACHE = 103,
    MENU_ID_STOPLOAD = 104,

    // Editing.
    MENU_ID_UNDO = 110,
    MENU_ID_REDO = 111,
    MENU_ID_CUT = 112,
    MENU_ID_COPY = 113,
    MENU_ID_PASTE = 114,
    MENU_ID_DELETE = 115,
    MENU_ID_SELECT_ALL = 116,

    // Miscellaneous.
    MENU_ID_FIND = 130,
    MENU_ID_PRINT = 131,
    MENU_ID_VIEW_SOURCE = 132,

    // All user-defined menu IDs should come between MENU_ID_USER_FIRST and
    // MENU_ID_USER_LAST to avoid overlapping the Chromium and CEF ID ranges
    // defined in the tools/gritsettings/resource_ids file.
    //MENU_ID_USER_FIRST = 26500,
    //MENU_ID_USER_LAST = 28500,

    MENU_ID_SHOW_DEVTOOLS = 28499,
    MENU_ID_HIDE_DEVTOOLS = 28498
}


class AnimatedContextMenuStrip : ContextMenuStrip
{
    #region Custom ToolStripProfessionalRenderer
    private class DefaultToolStripProfessionalRenderer : ToolStripProfessionalRenderer
    {
        class ContextMenuStripColorTableDark : ProfessionalColorTable
        {
            static readonly Color MENU_BACK_COLOR = Color.FromArgb(0x2e, 0x2e, 0x2e);

            static readonly Color MENU_BORDER_COLOR = Color.FromArgb(0x42, 0x42, blue: 0x42);

            static readonly Color MENU_HIGHLIGHT_COLOR = Color.FromArgb(0x4d, 0x4d, blue: 0x4d);

            public override Color MenuBorder => MENU_BORDER_COLOR;

            public override Color MenuItemBorder => Color.Transparent;



            public override Color MenuItemSelected => MENU_HIGHLIGHT_COLOR;

            public override Color MenuItemSelectedGradientBegin => MENU_HIGHLIGHT_COLOR;
            public override Color MenuItemSelectedGradientEnd => MENU_HIGHLIGHT_COLOR;


            public override Color ToolStripDropDownBackground => MENU_BACK_COLOR;
            public override Color ImageMarginGradientBegin => MENU_BACK_COLOR;
            public override Color ImageMarginGradientMiddle => MENU_BACK_COLOR;
            public override Color ImageMarginGradientEnd => MENU_BACK_COLOR;
        }

        class ContextMenuStripColorTableLight : ProfessionalColorTable
        {
            static readonly Color MENU_BACK_COLOR = Color.FromArgb(0xfa, 0xfa, 0xf9);

            static readonly Color MENU_BORDER_COLOR = Color.FromArgb(0xc7, 0xc7, blue: 0xc7);

            static readonly Color MENU_HIGHLIGHT_COLOR = Color.FromArgb(0xed, 0xed, blue: 0xed);

            public override Color MenuBorder => MENU_BORDER_COLOR;

            public override Color MenuItemBorder => Color.Transparent;



            public override Color MenuItemSelected => MENU_HIGHLIGHT_COLOR;

            public override Color MenuItemSelectedGradientBegin => MENU_HIGHLIGHT_COLOR;
            public override Color MenuItemSelectedGradientEnd => MENU_HIGHLIGHT_COLOR;


            public override Color ToolStripDropDownBackground => MENU_BACK_COLOR;
            public override Color ImageMarginGradientBegin => MENU_BACK_COLOR;
            public override Color ImageMarginGradientMiddle => MENU_BACK_COLOR;
            public override Color ImageMarginGradientEnd => MENU_BACK_COLOR;
        }




        public bool IsDarkMode { get; }


        public DefaultToolStripProfessionalRenderer(bool isDarkMode = false) :
            base(isDarkMode ? new ContextMenuStripColorTableDark() : new ContextMenuStripColorTableLight())
        {
            IsDarkMode = isDarkMode;
        }

        protected override void InitializeItem(ToolStripItem item)
        {
            item.Padding = new Padding(0, 6, 0, 6);
            base.InitializeItem(item);
        }

        protected override void Initialize(ToolStrip toolStrip)
        {


            base.Initialize(toolStrip);
        }



        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {


            base.OnRenderToolStripBackground(e);
        }



        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);
        }


        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            if (e.Image == null || !IsDarkMode)
            {
                base.OnRenderItemImage(e);

                return;
            }


            if (!e.Item.Enabled)
            {
                base.OnRenderItemImage(e);

                return;
            }

            var brightness = 1.5f; // no change in brightness
            var contrast = 1.0f; // twice the contrast
            var gamma = 1.0f; // no change in gamma

            var adjustedBrightness = brightness - 1.0f;

            float[][] ptsArray ={
            new float[] {contrast, 0, 0, 0, 0},
            new float[] {0, contrast, 0, 0, 0},
            new float[] {0, 0, contrast, 0, 0},
            new float[] {0, 0, 0, 1.0f, 0},
            new float[] {adjustedBrightness, adjustedBrightness,
        adjustedBrightness, 0, 1}};

            var imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray),
            ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);

            e.Graphics.DrawImage(e.Image, e.ImageRectangle, 0, 0, e.Image.Width, e.Image.Height, GraphicsUnit.Pixel, imageAttributes);

            //base.OnRenderItemImage(e);
        }





        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (IsDarkMode)
            {
                if (e.Item.Enabled)
                {
                    e.TextColor = Color.FromArgb(0xff, 0xff, 0xff);
                }
                else
                {
                    e.TextColor = Color.FromArgb(0xfa, 0xfa, 0xfa);
                }
            }
            else
            {
                if (e.Item.Enabled)
                {
                    e.TextColor = Color.FromArgb(0x1a, 0x1a, 0x1a);
                }
                else
                {
                    e.TextColor = Color.FromArgb(0xc3, 0xc3, 0xc3);
                }
            }

            //e.TextFormat &= ~TextFormatFlags.HidePrefix;
            //e.TextFormat |= TextFormatFlags.VerticalCenter;

            //var rect = ;
            //rect.Offset(-rect.X, -rect.Y);
            //e.TextRectangle = rect;

            var rect = new Rectangle(e.TextRectangle.X, (e.Item.Bounds.Height - e.TextRectangle.Height) / 2, e.TextRectangle.Width, e.TextRectangle.Height);


            e.TextRectangle = rect;


            base.OnRenderItemText(e);
        }

    }

    #endregion

    private const uint AW_HOR_POSITIVE = 0x1;
    private const uint AW_HOR_NEGATIVE = 0x2;
    private const uint AW_VER_POSITIVE = 0x4;
    private const uint AW_VER_NEGATIVE = 0x8;
    private const uint AW_CENTER = 0x10;
    private const uint AW_HIDE = 0x10000;
    private const uint AW_ACTIVATE = 0x20000;
    private const uint AW_SLIDE = 0x40000;
    private const uint AW_BLEND = 0x80000;



    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool DarkMode { get; set; } = false;

    public AnimatedContextMenuStrip()
    {
        RenderMode = ToolStripRenderMode.ManagerRenderMode;
        Renderer = new DefaultToolStripProfessionalRenderer();
    }

    public AnimatedContextMenuStrip(IContainer container) : this()
    {
        if (container == null)
        {
            throw new ArgumentNullException("container is null");
        }
        container.Add(this);
    }


    protected override void SetClientSizeCore(int x, int y)
    {
        base.SetClientSizeCore(x, y);
    }



    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, width, height, specified);
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        return base.GetPreferredSize(proposedSize);
    }


    protected override void OnOpening(CancelEventArgs e)
    {
        base.OnOpening(e);


        Opacity = 0;

        FadeIn();

        //User32.AnimateWindow(Handle, 50, AW_SLIDE | AW_VER_POSITIVE);
    }

    
    async void FadeIn()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(FadeIn));
            return;
        }


        Opacity += 0.1d;
        await Task.Delay(10);

        if (Opacity >= 1)
        {
            return;
        }
        ;

        FadeIn();
    }


}
