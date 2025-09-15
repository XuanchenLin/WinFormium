// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.


using WinFormium.ContextMenu;

namespace WinFormium;
internal partial class WebViewCore : IContextMenuHandler
{
    #region ContextMenuHelper
    /// <summary>
    /// Helper class for context menu related utility methods.
    /// </summary>
    class CefContextMenuHelper
    {
        const int MENU_ID_USER_FIRST = 26500;
        const int MENU_ID_USER_LAST = 28400;

        /// <summary>
        /// Determines whether the specified context menu identifier is an editing item.
        /// </summary>
        /// <param name="contextMenuId">The context menu identifier.</param>
        /// <returns><c>true</c> if the identifier is an editing item; otherwise, <c>false</c>.</returns>
        public static bool IsEditingItem(CefMenuIdentifiers contextMenuId)
        {
            var intValue = (int)contextMenuId;
            return IsEditingItem(intValue);
        }

        /// <summary>
        /// Determines whether the specified integer value is an editing item.
        /// </summary>
        /// <param name="intValue">The integer value of the context menu identifier.</param>
        /// <returns><c>true</c> if the value is an editing item; otherwise, <c>false</c>.</returns>
        public static bool IsEditingItem(int intValue)
        {
            return intValue >= (int)CefMenuIdentifiers.MENU_ID_UNDO && intValue <= (int)CefMenuIdentifiers.MENU_ID_SELECT_ALL;
        }

        /// <summary>
        /// Determines whether the specified integer value is a user-defined menu item.
        /// </summary>
        /// <param name="intValue">The integer value of the context menu identifier.</param>
        /// <returns><c>true</c> if the value is a user-defined item; otherwise, <c>false</c>.</returns>
        public static bool IsUserDefinedItem(int intValue)
        {
            return intValue > MENU_ID_USER_FIRST && intValue < MENU_ID_USER_LAST;
        }
    }
    #endregion

    /// <summary>
    /// Gets a value indicating whether a custom context menu handler is present.
    /// </summary>
    bool HasCustomContextMenuHandler => BrowserClient?.ContextMenuHandler != null;

    /// <inheritdoc/>
    void IContextMenuHandler.OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model)
    {

        BrowserClient?.ContextMenuHandler?.OnBeforeContextMenu(browser, frame, state, model);

        var idsShouldBeRemoved = new List<int>();

        if (SimplifyContextMenu)
        {
            for (nuint i = 0; i < model.Count; i++)
            {
                var ncmd = model.GetCommandIdAt(i);

                if (!CefContextMenuHelper.IsEditingItem(ncmd) && !CefContextMenuHelper.IsUserDefinedItem(ncmd))
                {
                    idsShouldBeRemoved.Add(ncmd);
                }
            }
        }

        foreach (var id in idsShouldBeRemoved)
        {
            model.Remove(id);
        }


        if (WinFormiumApp.Current.EnableDevToolsMenu)
        {
            if (model.Count > 0)
            {
                model.InsertSeparatorAt(model.Count);
            }

            model.InsertItemAt(model.Count, (int)CefMenuIdentifiers.MENU_ID_SHOW_DEVTOOLS, Resources.TextResource.WebView_ShowDevTools);
            model.InsertItemAt(model.Count, (int)CefMenuIdentifiers.MENU_ID_HIDE_DEVTOOLS, Resources.TextResource.WebView_HideDevTools);

            var hasDevTools = BrowserHost?.HasDevTools ?? false;

            model.SetEnabled((int)CefMenuIdentifiers.MENU_ID_SHOW_DEVTOOLS, !hasDevTools);
            model.SetEnabled((int)CefMenuIdentifiers.MENU_ID_HIDE_DEVTOOLS, hasDevTools);
        }

        var redoIdx = model.GetIndexOf((int)CefMenuIdentifiers.MENU_ID_REDO);

        if (redoIdx > -1)
        {
            model.InsertSeparatorAt((nuint)(redoIdx + 1));
        }
    }


    /// <inheritdoc/>
    bool IContextMenuHandler.OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams state, int commandId, CefEventFlags eventFlags)
    {

        switch ((CefMenuIdentifiers)commandId)
        {
            case CefMenuIdentifiers.MENU_ID_SHOW_DEVTOOLS:
                ShowDevTools();
                return true;
            case CefMenuIdentifiers.MENU_ID_HIDE_DEVTOOLS:
                HideDevTools();
                return true;
        }

        return BrowserClient?.ContextMenuHandler?.OnContextMenuCommand(browser, frame, state, commandId, eventFlags) ?? false;
    }


    /// <inheritdoc/>
    bool IContextMenuHandler.RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback)
    {
        if (HasCustomContextMenuHandler)
        {
            return BrowserClient?.ContextMenuHandler?.RunContextMenu(browser, frame, parameters, model, callback) ?? false;
        }

        if (!UseContextMenuStrip)
        {
            return false;
        }



        var entries = ContextMenuItem.FromCefMenuModel(model);

        ShowContextMenu(entries, parameters, callback);

        return true;
    }

    /// <inheritdoc/>
    void IContextMenuHandler.OnContextMenuDismissed(CefBrowser browser, CefFrame frame)
    {
        if (HasCustomContextMenuHandler)
        {
            BrowserClient?.ContextMenuHandler?.OnContextMenuDismissed(browser, frame);
            return;
        }
    }

    /// <inheritdoc/>
    bool IContextMenuHandler.OnQuickMenuCommand(CefBrowser browser, CefFrame frame, int commandId, CefEventFlags eventFlags)
    {
        return BrowserClient?.ContextMenuHandler?.OnQuickMenuCommand(browser, frame, commandId, eventFlags) ?? false;
    }

    /// <inheritdoc/>
    void IContextMenuHandler.OnQuickMenuDismissed(CefBrowser browser, CefFrame frame)
    {
        BrowserClient?.ContextMenuHandler?.OnQuickMenuDismissed(browser, frame);
    }

    /// <inheritdoc/>
    bool IContextMenuHandler.RunQuickMenu(CefBrowser browser, CefFrame frame, CefPoint location, CefSize size, CefQuickMenuEditStateFlags editStateFlags, CefRunQuickMenuCallback callback)
    {
        return BrowserClient?.ContextMenuHandler?.RunQuickMenu(browser, frame, location, size, editStateFlags, callback) ?? false;
    }

    #region CustomContextMenu

    /// <summary>
    /// Gets or sets the current instance of the animated browser context menu.
    /// </summary>
    internal AnimatedContextMenuStrip? BrowserContextMenuInstance { get; set; }
    

    ///<summary>
    /// Closes the currently displayed context menu if present and not handled by a custom handler.
    /// </summary>
    internal void CloseContextMenu()
    {
        if (HasCustomContextMenuHandler || BrowserContextMenuInstance is null) return;

        if (BrowserContextMenuInstance?.InvokeRequired ?? false)
        {
            BrowserContextMenuInstance?.Invoke(CloseContextMenu);
            return;
        }

        BrowserContextMenuInstance?.Close();

        BrowserContextMenuInstance = null;
    }

    /// <summary>
    /// Displays a custom context menu using the specified menu entries and callback.
    /// </summary>
    /// <param name="entries">The context menu items to display.</param>
    /// <param name="parameters">The context menu parameters.</param>
    /// <param name="callback">The callback to invoke when a menu item is selected or the menu is dismissed.</param>
    internal void ShowContextMenu(ContextMenuItem[] entries, CefContextMenuParams parameters, CefRunContextMenuCallback callback)
    {
        void AssignMenuItemIcon(ToolStripMenuItem item)
        {
            var itemData = (ContextMenuItem?)item.Tag;

            var hasDevTools = BrowserHost?.HasDevTools ?? false;

            if (itemData == null)
                return;

            switch (itemData.CommandId)
            {
                case (int)CefMenuIdentifiers.MENU_ID_COPY:
                    item.Image = Resources.Images.ContextMenu_Copy;
                    item.ShowShortcutKeys = true;
                    item.ShortcutKeys = Keys.Control | Keys.C;
                    break;
                case (int)CefMenuIdentifiers.MENU_ID_CUT:
                    item.Image = Resources.Images.ContextMenu_Cut;
                    item.ShowShortcutKeys = true;
                    item.ShortcutKeys = Keys.Control | Keys.X;
                    break;
                case (int)CefMenuIdentifiers.MENU_ID_PASTE:
                    item.Image = Resources.Images.ContextMenu_Paste;
                    item.ShowShortcutKeys = true;
                    item.ShortcutKeys = Keys.Control | Keys.V;
                    break;
                case (int)CefMenuIdentifiers.MENU_ID_UNDO:
                    item.Image = Resources.Images.ContextMenu_Undo;
                    item.ShowShortcutKeys = true;
                    item.ShortcutKeys = Keys.Control | Keys.Z;
                    break;
                case (int)CefMenuIdentifiers.MENU_ID_REDO:
                    item.Image = Resources.Images.ContextMenu_Redo;
                    item.ShowShortcutKeys = true;
                    item.ShortcutKeys = Keys.Control | Keys.Y;
                    break;
                case (int)CefMenuIdentifiers.MENU_ID_SHOW_DEVTOOLS when !hasDevTools:
                    item.Image = Resources.Images.ContextMenu_DevTools;
                    break;
                case (int)CefMenuIdentifiers.MENU_ID_HIDE_DEVTOOLS when hasDevTools:
                    item.Image = Resources.Images.ContextMenu_DevTools;
                    break;
            }
        }

        InvokeIfRequired(() =>
        {
            if (BrowserContextMenuInstance != null)
            {
                BrowserContextMenuInstance.Close();
                BrowserContextMenuInstance.Dispose();
                BrowserContextMenuInstance = null;
            }

            var menu = new AnimatedContextMenuStrip();

            foreach (var entry in entries)
            {
                if (entry.IsSeparator)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }
                else
                {
                    var item = new ToolStripMenuItem()
                    {
                        Text = entry.Label,
                        Enabled = entry.IsEnabled,
                        Checked = entry.IsChecked ?? false,
                        Tag = entry,
                    };
                    var cmdId = entry.CommandId;

                    item.Click += (_, _) =>
                    {
                        callback.Continue(cmdId, CefEventFlags.None);
                    };

                    AssignMenuItemIcon(item);

                    menu.Items.Add(item);
                }
            }

            menu.Closed += (_, args) =>
            {
                if (args.CloseReason != ToolStripDropDownCloseReason.ItemClicked)
                {
                    callback.Cancel();
                }
            };

            menu.Show(Control.MousePosition);

            BrowserContextMenuInstance = menu;
        });
    }


    #endregion
}
