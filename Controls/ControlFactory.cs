﻿using System.Collections.Generic;
using Editor.DataClasses.GameDataClasses;
using Button = System.Windows.Forms.Button;

namespace Editor.Controls;

public static class ControlFactory
{
   #region enums

   public enum ImageButtonType
   {
      GreenPlus,
      RedMinus,
      OrangePlus,
      RedX,
      Map
   }

   #endregion


   public static CollectionEditor GetCollectionEditor(string name, string mapModeName, ItemTypes itemTypes, List<string> comboBoxItems, Func<string, List<string>> onIndexSelectedFunc, Func<string, bool, List<string>> onAddedOrRemovedFunc, Func<string, List<string>> onNewCreated, Action<string> onDeleted, Action<string, string> onSingleRemoved)
   {
      CollectionEditor ce = new (name, mapModeName, itemTypes, onIndexSelectedFunc, onAddedOrRemovedFunc, onNewCreated, onDeleted, onSingleRemoved)
      {
         Margin = new(1),
         Dock = DockStyle.Fill
      };

      ce.SetComboBoxItems(comboBoxItems);

      return ce;
   }

   public static Button GetImageButton(ImageButtonType ibType, string toolTip)
   {
      Button button = new()
      {
         Text = string.Empty,
         Image = ibType switch
         {
            ImageButtonType.GreenPlus => Properties.Resources.GreenPlusBg,
            ImageButtonType.RedMinus => Properties.Resources.RedMinus,
            ImageButtonType.OrangePlus => Properties.Resources.OrangePlus,
            ImageButtonType.RedX => Properties.Resources.RedX,
            ImageButtonType.Map => Properties.Resources.map,
            _ => null
         },
         Size = new(26, 26),
         Dock = DockStyle.Fill,
         Margin = new(1, 3, 1, 3),
      };
      
      Globals.MapWindow.GeneralToolTip.SetToolTip(button, toolTip);

      return button;
   }

   public static NumberTextBox GetNumberTextBox()
   {
      return new ();
   }

   public static PannablePictureBox GetPannablePictureBox(ref Panel panel, MapWindow mapWindow)
   {
      return new (ref panel, mapWindow);
   }

   public static ExtendedComboBox GetExtendedComboBox(List<string> content, int selectedIndex = -1)
   {
      var ec = new ExtendedComboBox();
      if (content.Count == 0)
         return ec;
      ec.Items.AddRange([..content]);
      if (selectedIndex >= 0 && selectedIndex < ec.Items.Count)
         ec.SelectedIndex = selectedIndex;
      return ec;
   }

   public static ToolStripMenuItem GetToolStripMenuItem(string text, EventHandler onClick, Image image = null!)
   {
      return new(text, image, onClick);
   }

   public static SearchResultButton GetSearchResultButton(bool isProvince, int id, Tag tag)
   {
      return new ()
      {
         Margin = new (3),
         Width = 194,
         Text = isProvince ? Globals.Provinces[id].GetLocalisation() : Globals.Countries[tag].GetLocalisation(),
         Visible = true,
         IsProvince = isProvince,
         ProvinceId = id,
         CountryTag = tag
      };
   }

   public static TagComboBox GetTagComboBox()
   {
      return new ()
      {
         Margin = new(3, 1, 3, 3)
      };
   }

   public static ItemList GetItemList(ItemTypes itemType, List<string> items, string title)
   {
      var list = new ItemList(itemType, GetTagComboBox());
      list.InitializeItems(items);
      list.SetTitle(title);
      list.Margin = new(3, 1, 3, 3);
      return list;
   }

   public static ItemList GetItemListObjects(ItemTypes itemType, List<object> items, string title)
   {
      List<string> strings = [];

      foreach (var item in items)
      {
         var str = item.ToString();
         if (str != null)
            strings.Add(str);
      }

      if (strings.Count > 1)
         return GetItemList(itemType, strings, title);
      return new (itemType, GetTagComboBox());
   }

   public static ItemButton GetItemButton(string item, ItemTypes type)
   {
      return new (item, type) { Width = 43, Height = 25 };
   }

   public static ItemButton GetItemButtonLong(string item, ItemTypes type)
   {
      return new(item, type) { Width = 110, Height = 25 };
   }

   public static ItemButton GetTagItemButton(string item, ItemTypes type)
   {
      return new (item, type);
   }

   public static ItemButton GetStringItemButton(string item, ItemTypes type)
   {
      return new (item, type){Width = 75, Height = 25};
   }

   public static ExtendedComboBox GetExtendedComboBox(bool def = true)
   {
      if (def)
         return new ()
         {
            Margin = new(3, 1, 3, 3)
         };
      return new ();
   }



   public static ExtendedNumeric GetExtendedNumeric()
   {
      return new ()
      {
         Margin = new(3,1,3,3)
      };
   }
}