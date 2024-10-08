﻿using Editor.Events;
using Editor.Helper;

namespace Editor.DataClasses.MapModes;

public sealed class SuperRegionMapMode : MapMode
{
   public SuperRegionMapMode()
   {
      ProvinceEventHandler.OnSuperRegionRegionChanged += UpdateProvince!;
   }

   public override string GetMapModeName()
   {
      return "Super Regions";
   }

   public override Color GetProvinceColor(int id)
   {
      if (Globals.Provinces.TryGetValue(id, out var province))
         if (Globals.Areas.TryGetValue(province.Area, out var areas))
            if (Globals.Regions.TryGetValue(areas.Region, out var region))
               if (Globals.SuperRegions.TryGetValue(region.SuperRegion, out var superRegion))
                  return superRegion.Color;
      return Color.DarkGray;
   }

   public override string GetSpecificToolTip(int id)
   {
      if (Globals.Provinces.TryGetValue(id, out var province))
         if (Globals.Areas.TryGetValue(province.Area, out var areas))
            if (Globals.Regions.TryGetValue(areas.Region, out var region))
               if (Globals.SuperRegions.TryGetValue(region.SuperRegion, out var superRegion))
                  return $"Super Region: {superRegion.Name} ({Localisation.GetLoc(superRegion.Name)})";
      return "Super Region: [Unknown]";
   }
}