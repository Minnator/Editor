﻿using Editor.DataClasses.GameDataClasses;

namespace Editor.DataClasses.MapModes;

public sealed class ProvinceIdMapMode : MapMode
{
   
   public override MapModeType GetMapModeName()
   {
      return MapModeType.Province;
   }

   public override int GetProvinceColor(Province id)
   {
      return id.Color.ToArgb();
   }

   public override string GetSpecificToolTip(Province provinceId)
   {
      return $"{provinceId.Id} ({provinceId.GetLocalisation()})";
   }
}