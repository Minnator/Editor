﻿using System.Drawing;
using Editor.DataClasses.MapModes;
using Editor.Events;

namespace Editor.MapModes;

public class CenterOfTradeMapMode : MapMode
{
   //TODO read min an max from defines

   public override bool IsLandOnly => true;

   public CenterOfTradeMapMode()
   {
      // Subscribe to events to update the min and max values when a province's development changes
      ProvinceEventHandler.OnProvinceCenterOfTradeLevelChanged += UpdateProvince;
   }

   public override Color GetProvinceColor(int id)
   {
      if (Globals.SeaProvinces.Contains(id) || Globals.LakeProvinces.Contains(id))
         return Globals.Provinces[id].Color;

      return Globals.Provinces[id].CenterOfTrade switch
      {
         0 => Color.DimGray,
         1 => Color.FromArgb(0, 0, 255),
         2 => Color.FromArgb(0, 255, 0),
         3 => Color.FromArgb(255, 0, 0),
         _ => Color.FromArgb(255, 255, 255)
      };
   }

   public override string GetMapModeName()
   {
      return "Center of Trade";
   }

   public override string GetSpecificToolTip(int provinceId)
   {
      if (Globals.Provinces.TryGetValue(provinceId, out var province))
         return province.CenterOfTrade switch
         {
            0 => "No center of trade",
            1 => "Center of trade: [Level 1]",
            2 => "Center of trade: [Level 2]",
            3 => "Center of trade: [Level 3]",
            _ => "Center of trade: [Unknown]"
         };
      return "Center of Trade: [Unknown]";
   }
}