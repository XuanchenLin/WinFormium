// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

using Windows.Win32.UI.Input.Ime;

/// <summary>
/// Provides an implementation of <see cref="IRenderHandler"/> for off-screen rendering in WinFormium.
/// Handles IME, drag-and-drop, touch, and rendering-related events for the associated <see cref="WebViewCore"/>.
/// </summary>
internal partial class FormiumOffscreenRenderHandler
{
    /// <summary>
    /// Handles IME (Input Method Editor) operations for the off-screen render handler.
    /// Manages composition, caret, and IME window positioning for East Asian languages.
    /// </summary>
    private class WebViewOsrImeHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewOsrImeHandler"/> class.
        /// </summary>
        /// <param name="handler">The parent <see cref="FormiumOffscreenRenderHandler"/> instance.</param>
        public WebViewOsrImeHandler(FormiumOffscreenRenderHandler handler)
        {
            OsrHandler = handler;
        }

        /// <summary>
        /// Gets the parent <see cref="FormiumOffscreenRenderHandler"/> instance.
        /// </summary>
        public FormiumOffscreenRenderHandler OsrHandler { get; }

        /// <summary>
        /// Updates the composition range and character bounds for IME composition.
        /// </summary>
        /// <param name="selectRange">The selected range.</param>
        /// <param name="bounds">The character bounds.</param>
        public void ChangeCompositionRange(CefRange selectRange, IEnumerable<CefRectangle> bounds)
        {
            var scaleFactor = ScaleFactor;

            compositionRange = selectRange;

            var rects = new List<CefRectangle>();

            foreach (var rect in bounds)
            {
                var scaledBounds = new CefRectangle((int)(rect.X * scaleFactor), (int)(rect.Y * scaleFactor), (int)(rect.Width * scaleFactor), (int)(rect.Height * scaleFactor));
                rects.Add(scaledBounds);
            }

            compositionBounds = rects;

            MoveImeWindow();
        }

        /// <summary>
        /// Handles the IME set context message, disabling the UI composition window.
        /// </summary>
        /// <param name="m">The Windows message.</param>
        public void OnIMESetContext(ref Message m)
        {
            //var retval = (ulong)m.LParam;

            //retval &= ~ImeNative.ISC_SHOWUICOMPOSITIONWINDOW;

            //var lParam = (IntPtr)retval;

            //DefWindowProc(hWnd, (uint)m.Msg, (nuint)m.WParam, lParam);

            //var himc = ImeNative.ImmGetContext(Handle);
            //ImeNative.ImmAssociateContext(hWnd, himc);

            m.LParam = (IntPtr)((ulong)m.LParam & ~ISC_SHOWUICOMPOSITIONWINDOW);

            DefWindowProc((HWND)m.HWnd, (uint)m.Msg, (nuint)m.WParam, (nint)((ulong)m.LParam & ~ISC_SHOWUICOMPOSITIONWINDOW));

            //MoveImeWindow();
        }

        /// <summary>
        /// Handles the start of IME composition.
        /// </summary>
        public void OnImeStartComposition()
        {
            MoveImeWindow();
        }

        /// <summary>
        /// Handles IME composition messages, updating the browser host with composition or result text.
        /// </summary>
        /// <param name="m">The Windows message.</param>
        public void OnImeComposition(ref Message m)
        {
            var lParam = m.LParam;

            var browserHost = OsrHandler.BrowserHost;

            if (browserHost == null) return;

            if (GetResult((uint)lParam, out var textStr))
            {
                browserHost.ImeCommitText(textStr ?? string.Empty, new CefRange(int.MaxValue, int.MaxValue), 0);

                browserHost.ImeSetComposition(textStr ?? string.Empty, 0, new CefCompositionUnderline(), new CefRange(int.MaxValue, int.MaxValue), new CefRange(0, 0));

                browserHost.ImeFinishComposingText(false);
            }
            else
            {
                var underlines = new List<CefCompositionUnderline>();

                if (GetComposition((uint)lParam, out textStr, out var compostionStart, ref underlines))
                {
                    browserHost.ImeSetComposition(textStr, underlines.Count, underlines[0], new CefRange(int.MaxValue, int.MaxValue), new CefRange(compostionStart, compostionStart/* + textStr.Length*/));

                    //UpdateCaretPosition(compostionStart - 1);
                }
                else
                {
                    browserHost.ImeSetComposition(string.Empty, 1, new CefCompositionUnderline { }, new CefRange(0, 1), new CefRange(0, 1));

                    OnImeCancelComposition();
                }
            }
        }

        /// <summary>
        /// Cancels the current IME composition and resets the IME state.
        /// </summary>
        public void OnImeCancelComposition()
        {
            var browserHost = OsrHandler.BrowserHost;

            if (browserHost == null) return;

            browserHost?.ImeSetComposition(string.Empty, 0, new CefCompositionUnderline(), new CefRange(int.MaxValue, int.MaxValue), new CefRange(0, 0));

            browserHost?.ImeCommitText(string.Empty, new CefRange(int.MaxValue, int.MaxValue), 0);

            browserHost?.ImeFinishComposingText(false);

            browserHost?.ImeCancelComposition();
            //ResetComposition();
            //DestroyImeWindow();
        }

        /// <summary>
        /// Gets the window handle as IntPtr.
        /// </summary>
        internal IntPtr Handle => OsrHandler.WindowHandle;

        /// <summary>
        /// Determines whether the specified attribute is a selection attribute for IME composition.
        /// </summary>
        /// <param name="attribute">The attribute byte.</param>
        /// <returns>True if the attribute is a selection attribute; otherwise, false.</returns>
        internal bool IsSelectionAttribute(byte attribute)
        {
            return (attribute == ATTR_TARGET_CONVERTED || attribute == ATTR_TARGET_NOTCONVERTED);
        }

        /// <summary>
        /// Gets the selection range for the current IME composition.
        /// </summary>
        /// <param name="imc">The input context handle.</param>
        /// <param name="targetStart">The start index of the selection.</param>
        /// <param name="targetEnd">The end index of the selection.</param>
        internal unsafe void GetCompositionSelectionRange(HIMC imc, out int targetStart, out int targetEnd)
        {
            var attributeSize = ImmGetCompositionString(imc, IME_COMPOSITION_STRING.GCS_COMPATTR, null, 0);

            if (attributeSize > 0)
            {
                var buff = stackalloc byte[attributeSize];

                ImmGetCompositionString(imc, IME_COMPOSITION_STRING.GCS_COMPATTR, buff, (uint)attributeSize);

                int start, end;
                for (start = 0; start < attributeSize; ++start)
                {
                    if (IsSelectionAttribute(buff[start]))
                        break;
                }

                for (end = start; end < attributeSize; ++end)
                {
                    if (!IsSelectionAttribute(buff[end]))
                        break;
                }

                targetStart = start;
                targetEnd = end;
            }
            else
            {
                targetStart = 0;
                targetEnd = 0;
            }
        }

        /// <summary>
        /// Gets the composition underlines for the current IME composition.
        /// </summary>
        /// <param name="imc">The input context handle.</param>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <returns>A collection of <see cref="CefCompositionUnderline"/>.</returns>
        internal unsafe IEnumerable<CefCompositionUnderline> GetCompositionUnderlines(HIMC imc, int start, int end)
        {
            var clauseSize = ImmGetCompositionString(imc, IME_COMPOSITION_STRING.GCS_COMPCLAUSE, null, 0);

            var clauseLength = (int)clauseSize / sizeof(int);

            var result = new List<CefCompositionUnderline>();

            if (clauseLength > 0)
            {
                var clauseData = stackalloc byte[clauseSize];

                ImmGetCompositionString(imc, IME_COMPOSITION_STRING.GCS_COMPCLAUSE, clauseData, (uint)clauseSize);

                var buff = BytePtrToArray(clauseData, clauseSize);

                for (var i = 0; i < clauseLength - 1; i++)
                {
                    var from = BitConverter.ToInt32(buff, i * sizeof(int));
                    var to = BitConverter.ToInt32(buff, (i + 1) * sizeof(int));

                    var range = new CefRange(from, to);

                    var think = range.From >= start && range.To <= end;

                    var underline = new CefCompositionUnderline
                    {
                        Range = range,
                        Color = new CefColor(COLOR_UNDERLINE),
                        BackgroundColor = new CefColor(COLOR_BKCOLOR),
                        Thick = think,
                    };

                    result.Add(underline);
                }
            }

            return result;
        }

        /// <summary>
        /// Moves the IME window to the current caret or composition position.
        /// </summary>
        internal void MoveImeWindow()
        {
            if (OsrHandler.HostWindow.InvokeRequired)
            {
                OsrHandler.HostWindow.Invoke(new Action(MoveImeWindow));
                return;
            }

            if (GetFocus() != Handle)
            {
                return;
            }

            if (compositionBounds.Count == 0)
            {
                return;
            }

            var imc = ImmGetContext((HWND)Handle);

            //ImmAssociateContext((HWND)Handle, imc);

            CefRectangle rc = compositionBounds[0];

            var x = rc.X + rc.Width;
            var y = rc.Y + rc.Height;

            System.Diagnostics.Debug.WriteLine($"[MoveImeWindow] -> {x}:{y}");

            ImmSetCandidateWindow(imc, new CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = CFS_CANDIDATEPOS,
                ptCurrentPos = new Point(x, y),
            });

            ImmSetCompositionWindow(imc, new COMPOSITIONFORM
            {
                dwStyle = CFS_POINT,
                ptCurrentPos = new Point(x, y),
            });

            ImmSetCandidateWindow(imc, new CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = CFS_EXCLUDE,
                ptCurrentPos = new Point(x, y),
                rcArea = new RECT(rc.X, rc.Y, rc.X + rc.Width, rc.Y + rc.Height)
            });

            ImmReleaseContext((HWND)Handle, imc);
        }

        /// <summary>
        /// Gets composition underline information for the current IME composition.
        /// </summary>
        /// <param name="imc">The input context handle.</param>
        /// <param name="lparam">The lParam value from the message.</param>
        /// <param name="compositionText">The composition text.</param>
        /// <param name="compositionStart">The start index of the composition.</param>
        /// <returns>A list of <see cref="CefCompositionUnderline"/>.</returns>
        internal unsafe List<CefCompositionUnderline> GetCompositionInfo(HIMC imc, uint lparam, string compositionText, out int compositionStart)
        {
            var underlines = new List<CefCompositionUnderline>();
            var length = compositionText.Length;

            var targetStart = length;
            var targetEnd = length;

            if ((lparam & (uint)IME_COMPOSITION_STRING.GCS_COMPATTR) == (uint)IME_COMPOSITION_STRING.GCS_COMPATTR)
            {
                GetCompositionSelectionRange(imc, out targetStart, out targetEnd);
            }

            if (((lparam & CS_NOMOVECARET) != CS_NOMOVECARET) && ((lparam & (uint)IME_COMPOSITION_STRING.GCS_CURSORPOS) == (uint)IME_COMPOSITION_STRING.GCS_CURSORPOS))
            {
                var cursor = (int)ImmGetCompositionString(imc, IME_COMPOSITION_STRING.GCS_CURSORPOS, null, 0);
                compositionStart = cursor;
            }
            else
            {
                compositionStart = 0;
            }

            if ((lparam & (uint)IME_COMPOSITION_STRING.GCS_COMPCLAUSE) == (uint)IME_COMPOSITION_STRING.GCS_COMPCLAUSE)
            {
                underlines = GetCompositionUnderlines(imc, targetStart, targetEnd).ToList();
            }

            if (underlines.Count == 0)
            {
                var underline = new CefCompositionUnderline();
                underline.Color = new CefColor(COLOR_UNDERLINE);
                underline.BackgroundColor = new CefColor(COLOR_BKCOLOR);

                if (targetStart > 0)
                {
                    underline.Range = new CefRange(targetStart, targetEnd);
                    underline.Thick = true;
                    underlines.Add(underline);
                }

                if (targetEnd < length)
                {
                    underline.Range = new CefRange(targetEnd, length);
                    underline.Thick = false;
                    underlines.Add(underline);
                }
            }

            return underlines;
        }

        /// <summary>
        /// Retrieves a string from the IME composition string buffer.
        /// </summary>
        /// <param name="imc">The input context handle.</param>
        /// <param name="lparam">The lParam value from the message.</param>
        /// <param name="type">The composition string type.</param>
        /// <param name="result">The resulting string.</param>
        /// <returns>True if the string was retrieved; otherwise, false.</returns>
        internal unsafe bool GetString(HIMC imc, uint lparam, IME_COMPOSITION_STRING type, out string result)
        {
            if ((lparam & (uint)type) != (uint)type)
            {
                result = null!;
                return false;
            }

            var strlen = ImmGetCompositionString(imc, type, null, 0);

            if (strlen <= 0)
            {
                result = null!;
                return false;
            }

            var buff = stackalloc byte[strlen];

            ImmGetCompositionString(imc, type, buff, (uint)strlen);

            result = Encoding.Unicode.GetString(BytePtrToArray(buff, strlen));

            return true;
        }

        /// <summary>
        /// Gets the result string from the IME composition.
        /// </summary>
        /// <param name="lparam">The lParam value from the message.</param>
        /// <param name="result">The result string.</param>
        /// <returns>True if the result string was retrieved; otherwise, false.</returns>
        internal bool GetResult(uint lparam, out string? result)
        {
            var ret = false;

            var imc = ImmGetContext((HWND)Handle);

            if (imc != IntPtr.Zero)
            {
                ret = GetString(imc, lparam, IME_COMPOSITION_STRING.GCS_RESULTSTR, out result);

                ImmReleaseContext((HWND)Handle, imc);
            }
            else
            {
                result = null;
            }

            return ret;
        }

        /// <summary>
        /// Gets the current IME composition string and underline information.
        /// </summary>
        /// <param name="lparam">The lParam value from the message.</param>
        /// <param name="compositionText">The composition text.</param>
        /// <param name="compostionStart">The start index of the composition.</param>
        /// <param name="underlines">The underline information.</param>
        /// <returns>True if the composition string was retrieved; otherwise, false.</returns>
        internal bool GetComposition(uint lparam, out string compositionText, out int compostionStart, ref List<CefCompositionUnderline> underlines)
        {
            var imc = ImmGetContext((HWND)Handle);

            var ret = GetString(imc, lparam, IME_COMPOSITION_STRING.GCS_COMPSTR, out compositionText);

            if (ret)
            {
                underlines = GetCompositionInfo(imc, lparam, compositionText, out compostionStart);

                //isComposing = true;
            }
            else
            {
                compostionStart = 0;
            }

            ImmReleaseContext((HWND)Handle, imc);

            return ret;
        }

        private const int ATTR_TARGET_CONVERTED = 0x01;
        private const int ATTR_TARGET_NOTCONVERTED = 0x03;

        private const uint COLOR_UNDERLINE = 0xFF000000;
        private const uint COLOR_BKCOLOR = 0x00000000;

        private int languageCodeId;

        //private bool systemCaret;
        //private int cursorIndex = -1;
        //private bool isComposing = false;

        private CefRange compositionRange = new CefRange();

        private List<CefRectangle> compositionBounds = new List<CefRectangle>();

        /// <summary>
        /// Gets the current scale factor for DPI scaling.
        /// </summary>
        private float ScaleFactor => OsrHandler.ScaleFactor;

        /// <summary>
        /// Gets the primary language ID from the keyboard layout ID.
        /// </summary>
        /// <param name="lgid">The keyboard layout ID.</param>
        /// <returns>The primary language ID.</returns>
        private int PrimaryLangId(int lgid)
        {
            return lgid & 0x3ff;
        }
    }

    /// <inheritdoc/>
    void IRenderHandler.OnVirtualKeyboardRequested(CefBrowser browser, CefTextInputMode inputMode)
    {
        if (inputMode == CefTextInputMode.None)
        {
            SetFocusOnEditableElement(false);
        }
        else
        {
            SetFocusOnEditableElement(true);
        }
    }

    /// <inheritdoc/>
    void IRenderHandler.OnTouchHandleStateChanged(CefBrowser browser, CefTouchHandleState state)
    {
    }

    /// <inheritdoc/>
    void IRenderHandler.OnTextSelectionChanged(CefBrowser browser, string selectedText, CefRange selectedRange)
    {
    }

    /// <inheritdoc/>
    void IRenderHandler.OnImeCompositionRangeChanged(CefBrowser browser, CefRange selectedRange, CefRectangle[] characterBounds)
    {
        ImeHandler.ChangeCompositionRange(selectedRange, characterBounds);
    }

    /// <summary>
    /// The IME handler for this render handler.
    /// </summary>
    private WebViewOsrImeHandler ImeHandler;

    /// <summary>
    /// Indicates whether the current focus is on an editable field.
    /// </summary>
    private bool _isOnEditableField = false;

    /// <summary>
    /// Gets the browser host for the associated webview.
    /// </summary>
    private CefBrowserHost? BrowserHost => _webview.BrowserHost;

    /// <summary>
    /// Sets the focus state for editable elements and updates the IME mode.
    /// </summary>
    /// <param name="onEditableElement">True if focus is on an editable element; otherwise, false.</param>
    private void SetFocusOnEditableElement(bool onEditableElement)
    {
        if (HostWindow == null) return;

        _isOnEditableField = onEditableElement;

        _webview.InvokeIfRequired(() =>
        {
            if (onEditableElement == true)
            {
                HostWindow.ImeMode = ImeMode.Inherit;
            }
            else
            {
                HostWindow.ImeMode = ImeMode.Disable;
            }
        });
    }

    /// <summary>
    /// Registers input event handlers for the host window.
    /// </summary>
    /// <param name="control">The host form control.</param>
    private void RegisterHostWindowInputEvents(Form control)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new Action(() => RegisterHostWindowInputEvents(control)));
            return;
        }

        control.MouseMove += (_, e) => HandleMouseMove(e.AsCefMouseEvent(ScaleFactor));
        control.MouseLeave += (_, e) => HandleMouseLeave();
        control.MouseClick += (_, e) => HandleMouseClick(e.AsCefMouseEvent(ScaleFactor), e.Button, e.Clicks);
        control.MouseDown += (_, e) => HandleMouseDown(e.AsCefMouseEvent(ScaleFactor), e.Button.AsCefMouseButtonType(), e.Clicks);
        control.MouseUp += (_, e) => HandleMouseUp(e.AsCefMouseEvent(ScaleFactor), e.Button.AsCefMouseButtonType(), e.Clicks);
        control.MouseWheel += (_, e) => HandleMouseWheel(e.AsCefMouseEvent(ScaleFactor), e.Delta);

        control.KeyDown += (_, e) => HandleKeyUpDown(e.AsCefKeyEvent(false));
        control.KeyUp += (_, e) => HandleKeyUpDown(e.AsCefKeyEvent(true));
        control.KeyPress += (_, e) => HandleKeyPress(e);

        control.DragEnter += (_, e) => HandleDragEnter(e);
        control.DragDrop += (_, e) => HandleDragDrop(e);
        control.DragLeave += (_, e) => HandleDragLeave(e);
        control.DragOver += (_, e) => HandleDragOver(e);
    }

    /// <summary>
    /// Handles the drag over event.
    /// </summary>
    /// <param name="e">The drag event arguments.</param>
    private void HandleDragOver(DragEventArgs e)
    {
    }

    /// <summary>
    /// Handles the drag leave event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    private void HandleDragLeave(EventArgs e)
    {
    }

    /// <summary>
    /// Handles the drag drop event.
    /// </summary>
    /// <param name="e">The drag event arguments.</param>
    private void HandleDragDrop(DragEventArgs e)
    {
    }

    /// <summary>
    /// Handles the drag enter event.
    /// </summary>
    /// <param name="e">The drag event arguments.</param>
    private void HandleDragEnter(DragEventArgs e)
    {
    }

    /// <summary>
    /// Handles key up and key down events, forwarding them to the browser host.
    /// </summary>
    /// <param name="cefKeyEvent">The CEF key event.</param>
    private void HandleKeyUpDown(CefKeyEvent cefKeyEvent)
    {
        cefKeyEvent.FocusOnEditableField = _isOnEditableField;
        BrowserHost?.SendKeyEvent(cefKeyEvent);
    }

    /// <summary>
    /// Handles key press events, forwarding them to the browser host.
    /// </summary>
    /// <param name="e">The key press event arguments.</param>
    private void HandleKeyPress(KeyPressEventArgs e)
    {
        e.Handled = true;
        var keyEvent = new CefKeyEvent
        {
            EventType = CefKeyEventType.Char,
            WindowsKeyCode = e.KeyChar,
            Character = e.KeyChar,
            UnmodifiedCharacter = e.KeyChar,
            FocusOnEditableField = _isOnEditableField,
        };

        BrowserHost?.SendKeyEvent(keyEvent);
    }

    /// <summary>
    /// Handles mouse move events, forwarding them to the browser host.
    /// </summary>
    /// <param name="cefMouseEvent">The CEF mouse event.</param>
    private void HandleMouseMove(CefMouseEvent cefMouseEvent)
    {
        BrowserHost?.SendMouseMoveEvent(cefMouseEvent, false);
    }

    /// <summary>
    /// Handles mouse leave events, forwarding them to the browser host.
    /// </summary>
    private void HandleMouseLeave()
    {
        BrowserHost?.SendMouseMoveEvent(new CefMouseEvent(), false);
    }

    /// <summary>
    /// Handles mouse wheel events, forwarding them to the browser host.
    /// </summary>
    /// <param name="cefMouseEvent">The CEF mouse event.</param>
    /// <param name="delta">The wheel delta.</param>
    private void HandleMouseWheel(CefMouseEvent cefMouseEvent, int delta)
    {
        BrowserHost?.SendMouseWheelEvent(cefMouseEvent, 0, delta);
    }

    /// <summary>
    /// Handles mouse down events, forwarding them to the browser host.
    /// </summary>
    /// <param name="cefMouseEvent">The CEF mouse event.</param>
    /// <param name="cefMouseButtonType">The mouse button type.</param>
    /// <param name="clicks">The number of clicks.</param>
    private void HandleMouseDown(CefMouseEvent cefMouseEvent, CefMouseButtonType cefMouseButtonType, int clicks)
    {
        HostWindow.Focus();
        BrowserHost?.SendMouseClickEvent(cefMouseEvent, cefMouseButtonType, false, clicks);
    }

    /// <summary>
    /// Handles mouse up events, forwarding them to the browser host.
    /// </summary>
    /// <param name="cefMouseEvent">The CEF mouse event.</param>
    /// <param name="cefMouseButtonType">The mouse button type.</param>
    /// <param name="clicks">The number of clicks.</param>
    private void HandleMouseUp(CefMouseEvent cefMouseEvent, CefMouseButtonType cefMouseButtonType, int clicks)
    {
        BrowserHost?.SendMouseClickEvent(cefMouseEvent, cefMouseButtonType, true, clicks);
    }

    /// <summary>
    /// Handles mouse click events, including navigation for XButton1 and XButton2.
    /// </summary>
    /// <param name="cefMouseEvent">The CEF mouse event.</param>
    /// <param name="buttons">The mouse buttons.</param>
    /// <param name="clicks">The number of clicks.</param>
    private void HandleMouseClick(CefMouseEvent cefMouseEvent, MouseButtons buttons, int clicks)
    {
        if (buttons == MouseButtons.XButton1)
        {
            if (_webview.CanGoBack == true)
                _webview.GoBack();
        }

        if (buttons == MouseButtons.XButton2)
        {
            if (_webview.CanGoForward == true)
                _webview.GoForward();
        }
    }

    // 将 byte* 转换为 byte[] 的方法
    private static unsafe byte[] BytePtrToArray(byte* ptr, int length)
    {
        var arr = new byte[length];
        for (int i = 0; i < length; i++)
        {
            arr[i] = ptr[i];
        }
        return arr;
    }
}

/// <summary>
/// Provides extension methods for converting Windows Forms input events to CEF input event types.
/// </summary>
internal static class InputExtensions
{
    /// <summary>
    /// Converts a <see cref="MouseButtons"/> value to a <see cref="CefMouseButtonType"/> value.
    /// </summary>
    /// <param name="button">The mouse button to convert.</param>
    /// <returns>The corresponding <see cref="CefMouseButtonType"/> value.</returns>
    public static CefMouseButtonType AsCefMouseButtonType(this MouseButtons button)
    {
        switch (button)
        {
            case MouseButtons.Middle:
                return CefMouseButtonType.Middle;

            case MouseButtons.Right:
                return CefMouseButtonType.Right;

            default:
                return CefMouseButtonType.Left;
        }
    }

    /// <summary>
    /// Converts a <see cref="MouseEventArgs"/> instance to a <see cref="CefMouseEvent"/> instance, applying the specified scale factor.
    /// </summary>
    /// <param name="eventArgs">The mouse event arguments.</param>
    /// <param name="scaleFactor">The DPI scale factor to apply to the coordinates. Default is 1.0.</param>
    /// <returns>A <see cref="CefMouseEvent"/> representing the mouse event.</returns>
    public static CefMouseEvent AsCefMouseEvent(this MouseEventArgs eventArgs, float scaleFactor = 1.0f)
    {
        var cursorPos = eventArgs.Location;
        var modifiers = CefEventFlags.None;

        if (eventArgs.Button == MouseButtons.Left)
        {
            modifiers |= CefEventFlags.LeftMouseButton;
        }
        else if (eventArgs.Button == MouseButtons.Right)
        {
            modifiers |= CefEventFlags.RightMouseButton;
        }
        else if (eventArgs.Button == MouseButtons.Middle)
        {
            modifiers |= CefEventFlags.MiddleMouseButton;
        }

        return new CefMouseEvent((int)Math.Floor(cursorPos.X / scaleFactor), (int)Math.Floor(cursorPos.Y / scaleFactor), modifiers);
    }

    /// <summary>
    /// Converts a <see cref="KeyEventArgs"/> instance to a <see cref="CefKeyEvent"/> instance.
    /// </summary>
    /// <param name="eventArgs">The key event arguments.</param>
    /// <param name="isKeyUp">Indicates whether the event is a key up event.</param>
    /// <returns>A <see cref="CefKeyEvent"/> representing the key event.</returns>
    public static CefKeyEvent AsCefKeyEvent(this KeyEventArgs eventArgs, bool isKeyUp)
    {
        var modifiers = CefEventFlags.None;
        if (eventArgs.Shift)
        {
            modifiers |= CefEventFlags.ShiftDown;
        }
        if (eventArgs.Control)
        {
            modifiers |= CefEventFlags.ControlDown;
        }
        if (eventArgs.Alt)
        {
            modifiers |= CefEventFlags.AltDown;
        }

        return new CefKeyEvent
        {
            EventType = isKeyUp ? CefKeyEventType.KeyUp : CefKeyEventType.RawKeyDown,
            WindowsKeyCode = eventArgs.KeyValue,
            NativeKeyCode = 0,
            Modifiers = modifiers,
            IsSystemKey = false,
        };
    }
}