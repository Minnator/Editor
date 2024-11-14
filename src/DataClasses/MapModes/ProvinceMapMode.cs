﻿using Editor.DataClasses.GameDataClasses;

namespace Editor.DataClasses.MapModes;

public sealed class ProvinceMapMode : MapMode
{
   public ProvinceMapMode()
   {
      Province.ColorChanged += UpdateComposite<Province>;
   }

   public override MapModeType GetMapModeName()
   {
      return MapModeType.Province;
   }

   public override int GetProvinceColor(Province provinceId)
   {
      return provinceId.Color.ToArgb();
   }

   public override string GetSpecificToolTip(Province provinceId)
   {
      return $"Province: {provinceId.Id} ({provinceId.GetLocalisation()})";
   }
}