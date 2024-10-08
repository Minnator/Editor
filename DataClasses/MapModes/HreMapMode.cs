﻿using Editor.Events;

namespace Editor.DataClasses.MapModes
{
   public class HreMapMode : MapMode
   {
      public override bool IsLandOnly => true;

      public HreMapMode()
      {
         ProvinceEventHandler.OnProvinceIsHreChanged += UpdateProvince!;
      }

      public override Color GetProvinceColor(int id)
      {
         if (Globals.Provinces.TryGetValue(id, out var province))
            return province.IsHre ? Color.Green : Color.DimGray;
         return Color.DimGray;
      }

      public override string GetMapModeName()
      {
         return "HRE";
      }

      public override string GetSpecificToolTip(int provinceId)
      {
         if (Globals.Provinces.TryGetValue(provinceId, out var province))
            return province.IsHre ? "HRE" : "Not HRE";
         return "No province";
      }
   }
}