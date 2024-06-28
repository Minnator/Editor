﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Editor.Helper;
using Editor.Loading;

namespace Editor.MapModes;

public class ProvinceMapMode : IMapMode
{
   public Bitmap Bitmap { get; set; } = null!;

   public ProvinceMapMode()
   {
      RenderMapMode(GetProvinceColor);
   }

   public void RenderMapMode(Func<int, Color> method)
   {
      Bitmap?.Dispose();
      Bitmap = BitMapHelper.GenerateBitmapFromProvinces(GetProvinceColor);
      MapDrawHelper.DrawAllProvinceBorders(Bitmap, Color.Black);
   }

   public string GetMapModeName()
   {
      return "Provinces";
   }

   public Color GetProvinceColor(int provinceId)
   {
      return Globals.Provinces[provinceId].Color;
   }

   // We don't need these methods for this map mode as it is not interactive
   public void Update(Rectangle rect)
   {
   }

   public void Update(List<int> ids)
   {
   }

   public void Update(int id)
   {
   }
}