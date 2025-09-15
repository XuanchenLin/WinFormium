// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.ContextMenu;
class ContextMenuItem
{
    public bool IsSeparator { get; set; }

    public string Label { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public bool? IsChecked { get; set; }

    public ContextMenuItem[]? SubEntries { get; set; }

    public int CommandId { get; set; }

    internal static ContextMenuItem[] FromCefMenuModel(CefMenuModel model)
    {
        var items = new List<ContextMenuItem>();

        for (nuint i = 0; i < model.Count; i++)
        {
            
            var entry = new ContextMenuItem
            {
                Label = model.GetLabelAt(i) ?? "",
                IsEnabled = model.IsEnabledAt(i),
                CommandId = model.GetCommandIdAt(i)

            };

            switch (model.GetItemTypeAt(i))
            {
                case CefMenuItemType.Separator:
                    entry.IsSeparator = true;
                    break;

                case CefMenuItemType.Check:
                    entry.IsChecked = model.IsCheckedAt(i);
                    break;
            }


            var subMenuModel = model.GetSubMenuAt(i);
            if (subMenuModel != null)
            {
                entry.SubEntries = FromCefMenuModel(subMenuModel);
            }

            items.Add(entry);

            
        }

        return items.ToArray();
    }

}
