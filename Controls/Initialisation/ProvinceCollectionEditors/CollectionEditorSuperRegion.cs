﻿using Editor.DataClasses.Commands;
using Editor.Helper;

namespace Editor.Controls.Initialisation.ProvinceCollectionEditors
{
   public static class CollectionEditorSuperRegion
   {
      public static List<string> SuperRegionSelected(string s)
      {
         Globals.Selection.Clear();
         if (!Globals.SuperRegions.TryGetValue(s, out var superRegion))
            return [];

         Globals.Selection.AddRange(superRegion.GetProvinceIds());
         if (Globals.MapWindow.FocusSelectionCheckBox.Checked)
            Globals.Selection.FocusSelection();
         return superRegion.Regions;
      }
      
      public static List<string> ModifyExitingSuperRegion(string s, bool b)
      {
         if (!Globals.SuperRegions.TryGetValue(s, out var superRegion))
            return [];

         Globals.HistoryManager.AddCommand(new CModifyExistingSuperRegion(s, ProvinceCollectionHelper.GetRegionNamesFromProvinces(Globals.Selection.GetSelectedProvinces), b));

         return superRegion.Regions;
      }

      public static List<string> CreateNewSuperRegion(string s)
      {
         if (Globals.SuperRegions.TryGetValue(s, out _))
            return [];

         Globals.HistoryManager.AddCommand(new CAddNewSuperRegion(s, ProvinceCollectionHelper.GetRegionNamesFromProvinces(Globals.Selection.GetSelectedProvinces), Globals.MapWindow.SuperRegionEditingGui));

         if (Globals.SuperRegions.TryGetValue(s, out var newSuperRegion))
            return newSuperRegion.Regions;
         return [];
      }

      public static void DeleteSuperRegion(string s)
      {
         if (!Globals.SuperRegions.TryGetValue(s, out _))
            return;

         Globals.HistoryManager.AddCommand(new CDeleteSuperRegion(s, Globals.MapWindow.SuperRegionEditingGui));
      }

      public static void SingleItemModified(string s, string idStr)
      {
         if (!Globals.SuperRegions.TryGetValue(s, out _) || !Globals.Regions.ContainsKey(idStr))
            return;

         Globals.HistoryManager.AddCommand(new CModifyExistingSuperRegion(s, [idStr], false));
      }
      
   }
}