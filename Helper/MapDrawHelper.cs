﻿using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using ABI.Windows.Foundation;
using Editor.DataClasses.GameDataClasses;
using Point = System.Drawing.Point;

namespace Editor.Helper;

public static class MapDrawHelper
{
   // ================================ Public Methods ================================
   // Draws the given Array of Points on the given Bitmap with the given Color
   public static Rectangle DrawOnMap(Rectangle rect, Point[] points, Color color, Bitmap bmp)
   {
      if (points.Length == 0)
         return rect;
      switch (bmp.PixelFormat)
      {
         case PixelFormat.Format24bppRgb when points.Length > 4000:
            DrawPixels24BppParallel(rect, points, color, bmp);
            break;
         case PixelFormat.Format24bppRgb:
            DrawPixels24Bpp(rect, points, color, bmp);
            break;
         case PixelFormat.Format32bppArgb when points.Length > 4000:
            DrawPixels32BppParallel(rect, points, color, bmp);
            break;
         case PixelFormat.Format32bppArgb:
            DrawPixels32Bpp(rect, points, color, bmp);
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(bmp.PixelFormat), "Unknown Bitmap format.");
      }
      return rect;
   }

   public static Rectangle DrawOnMap(ICollection<int> ids, Func<int, Color> func, Bitmap bmp)
   {
      if (ids.Count == 0)
         return Rectangle.Empty;

      List<Rectangle> rects;

      switch (bmp.PixelFormat)
      {
         case PixelFormat.Format24bppRgb when ids.Count > Environment.ProcessorCount:
            DrawPixels24BppParallel(ids, func, bmp, out rects);
            break;
         case PixelFormat.Format24bppRgb:
            DrawPixels24Bpp(ids, func, bmp, out rects);
            break;
         case PixelFormat.Format32bppArgb when ids.Count > Environment.ProcessorCount:
            DrawPixels32BppParallel(ids, func, bmp, out rects);
            break;
         case PixelFormat.Format32bppArgb:
            DrawPixels32Bpp(ids, func, bmp, out rects);
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(bmp.PixelFormat), "Unknown Bitmap format.");
      }
      return Geometry.GetBounds(rects);
   }

   // Draws the border of the given province on the given Bitmap with the given Color
   public static Rectangle DrawProvinceBorder(int provinceIde, Color color, Bitmap bmp)
   {
      var province = Globals.Provinces[provinceIde];
      var points = new Point[province.BorderCnt];
      Array.Copy(Globals.BorderPixels, province.BorderPtr, points, 0, province.BorderCnt);
      return DrawOnMap(province.Bounds, points, color, bmp);
   }

   public static Rectangle DrawProvince(int provincePtr, Color color, Bitmap bmp)
   {
      var province = Globals.Provinces[provincePtr];
      var points = new Point[province.PixelCnt];
      Array.Copy(Globals.Pixels, province.PixelPtr, points, 0, province.PixelCnt);
      return DrawOnMap(province.Bounds, points, color, bmp);
   }

   public static void DrawAllProvinceBorders(Bitmap bmp, Color color)
   {
      DrawOnMap(new (0, 0, bmp.Width, bmp.Height), Globals.BorderPixels, color, bmp);
   }

   public static Rectangle DrawProvinceCollection (int[] provinceIds, Color color, Bitmap bmp, bool forceSingleDraw)
   {
      if (forceSingleDraw || provinceIds.Length > 20)
         return DrawProvinceCollection(provinceIds, color, bmp);
      List<Rectangle> rects = [];
      foreach (var provinceId in provinceIds)
      {
         DrawProvince(provinceId, Globals.Provinces[provinceId].Color, bmp);
         rects.Add(Globals.Provinces[provinceId].Bounds);
      }
      return Geometry.GetBounds(rects);
   }
   
   public static Rectangle DrawBorderCollection (int[] provinceIds, Color color, Bitmap bmp, bool forceSingleDraw)
   {
      if (forceSingleDraw || provinceIds.Length > 20)
         return DrawBorderCollection(provinceIds, color, bmp);
      List<Rectangle> rects = [];
      foreach (var provinceId in provinceIds)
      {
         DrawProvinceBorder(provinceId, color, bmp);
         rects.Add(Globals.Provinces[provinceId].Bounds);
      }
      return Geometry.GetBounds(rects);
   }

   private static Rectangle DrawBorderCollection (int[] provinceIds, Color color, Bitmap bmp)
   {
     var rects = provinceIds.Select(ptr => Globals.Provinces[ptr].Bounds).ToList();
      return DrawOnMap(Geometry.GetBounds(rects), Geometry.GetAllBorderPoints(provinceIds), color, bmp);
   }

   private static Rectangle DrawProvinceCollection (int[] provinceIds, Color color, Bitmap bmp)
   {
      var rects = provinceIds.Select(ptr => Globals.Provinces[ptr].Bounds).ToList();
      Geometry.GetAllPixelPoints(provinceIds, out var points);
      return DrawOnMap(Geometry.GetBounds(rects), points, color, bmp);
   }

   // ------------------------------ 24bpp ------------------------------
   #region 24bpp Drawing
   // !!24bpp!! Draws the given Array of Points on the given Bitmap with the given Color in parallel

   private static void DrawPixels24BppParallel(ICollection<int> ids, Func<int, Color> func, Bitmap bmp, out List<Rectangle> rects)
   {
      rects = [];
      foreach (var id in ids)
      {
         var province = Globals.Provinces[id];
         var points = new Point[province.PixelCnt];
         Array.Copy(Globals.Pixels, province.PixelPtr, points, 0, province.PixelCnt);
         DrawPixels24BppParallel(province.Bounds, points, func(id), bmp);
         rects.Add(province.Bounds);
      }
   }

   private static void DrawPixels24Bpp (ICollection<int> ids, Func<int, Color> func, Bitmap bmp, out List<Rectangle> rects)
   {
      rects = [];
      foreach (var id in ids)
      {
         var province = Globals.Provinces[id];
         var points = new Point[province.PixelCnt];
         Array.Copy(Globals.Pixels, province.PixelPtr, points, 0, province.PixelCnt);
         DrawPixels24Bpp(province.Bounds, points, func(id), bmp);
         rects.Add(province.Bounds);
      }
   }
   
   private static void DrawPixels24BppParallel(Rectangle rect, Point[] points, Color color, Bitmap bmp)
   {
      var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);

      unsafe
      {
         var r = color.R;
         var g = color.G;
         var b = color.B;
         var scan0 = (byte*)bmpData.Scan0.ToPointer();

         Parallel.ForEach(points, point =>
         {
            var index = (point.Y - rect.Y) * bmpData.Stride + (point.X - rect.X) * 3;

            scan0[index]     = b;       // Blue component
            scan0[index + 1] = g;   // Green component
            scan0[index + 2] = r;   // Red component
         });
      }

      bmp.UnlockBits(bmpData);
   }
   // !!24bpp!! Draws the given Array of Points on the given Bitmap with the given Color
   private static void DrawPixels24Bpp(Rectangle rect, Point[] points, Color color, Bitmap bmp)
   {
      var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);

      unsafe
      {
         var r = color.R;
         var g = color.G;
         var b = color.B;
         var scan0 = (byte*)bmpData.Scan0.ToPointer();



         foreach (var point in points)
         {
            var index = (point.Y - rect.Y) * bmpData.Stride + (point.X - rect.X) * 3;

            scan0[index] = b;       // Blue component
            scan0[index + 1] = g;   // Green component
            scan0[index + 2] = r;   // Red component
         }
      }

      bmp.UnlockBits(bmpData);
   }
   #endregion

   // ------------------------------ 32bpp ------------------------------
   #region 32bpp Drawing
   // !!32bpp!! Draws the given Array of Points on the given Bitmap with the given Color
   
   private static void DrawPixels32Bpp(ICollection<int> ids, Func<int, Color> func, Bitmap bmp, out List<Rectangle> rects)
   {
      rects = [];
      foreach (var id in ids)
      {
         var province = Globals.Provinces[id];
         var points = new Point[province.PixelCnt];
         Array.Copy(Globals.Pixels, province.PixelPtr, points, 0, province.PixelCnt);
         DrawPixels32Bpp(province.Bounds, points, func(id), bmp);
         rects.Add(province.Bounds);
      }
   }

   private static void DrawPixels32BppParallel(ICollection<int> ids, Func<int, Color> func, Bitmap bmp, out List<Rectangle> rects)
   {
      rects = [];
      foreach (var id in ids)
      {
         var province = Globals.Provinces[id];
         var points = new Point[province.PixelCnt];
         Array.Copy(Globals.Pixels, province.PixelPtr, points, 0, province.PixelCnt);
         DrawPixels32BppParallel(province.Bounds, points, func(id), bmp);
         rects.Add(province.Bounds);
      }
   }

   private static void DrawPixels32Bpp(Rectangle rect, Point[] points, Color color, Bitmap bmp)
   {
      var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

      unsafe
      {
         var a = color.A;
         var r = color.R;
         var g = color.G;
         var b = color.B;
         var scan0 = (byte*)bmpData.Scan0.ToPointer();

         foreach (var point in points)
         {
            var index = (point.Y - rect.Y) * bmpData.Stride + (point.X - rect.X) * 4;

            scan0[index] = b;       // Blue component
            scan0[index + 1] = g;   // Green component
            scan0[index + 2] = r;   // Red component
            scan0[index + 3] = a;   // Alpha component
         }
      }

      bmp.UnlockBits(bmpData);
   }
   // !!32bpp!! Draws the given Array of Points on the given Bitmap with the given Color in parallel
   private static void DrawPixels32BppParallel(Rectangle rect, Point[] points, Color color, Bitmap bmp)
   {
      var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

      try
      {
         unsafe
         {
            var a = color.A;
            var r = color.R;
            var g = color.G;
            var b = color.B;
            var scan0 = (byte*)bmpData.Scan0.ToPointer();

            Parallel.ForEach(points, point =>
            {
               var index = (point.Y - rect.Y) * bmpData.Stride + (point.X - rect.X) * 4;

               scan0[index] = b; // Blue component
               scan0[index + 1] = g; // Green component
               scan0[index + 2] = r; // Red component
               scan0[index + 3] = a; // Alpha component
            });
         }
      }
      finally 
      {
         bmp.UnlockBits(bmpData);
      }
   }
   #endregion

   public static Rectangle DrawStripes(Color color, List<int> ids, Bitmap bmp)
   {
      var rects = new Rectangle[ids.Count];
      for (var index = 0; index < ids.Count; index++)
      {
         var province = Globals.Provinces[ids[index]];
         Geometry.GetStripesArray(province, out var stripePixels);
         rects[index] = DrawOnMap(province.Bounds, stripePixels, color, bmp);
      }

      return Geometry.GetBounds([..rects]);
   }

   public static void DrawOccupations(bool rebelsOnly, Bitmap bmp)
   {
      foreach (var id in Globals.LandProvinces)
      {
         var province = Globals.Provinces[id];
         if (rebelsOnly && !province.HasRevolt) // Has no rebels but we only want to show rebels
            continue;
         if (!rebelsOnly && province is { IsNonRebelOccupied: false, HasRevolt: false }) // has neither rebels nor occupation, but we want to show some
            continue;

         if (!Geometry.GetIfHasStripePixels(province, rebelsOnly, out var stripePixels))
            continue;

         DrawOnMap(province.Bounds, stripePixels, province.GetOccupantColor, bmp);
      }
   }
   
   public static void DrawAllCapitals(Bitmap bmp)
   {
      List<Point> capitals = [];
      foreach (var id in Globals.Capitals)
         capitals.Add(Globals.Provinces[id].Center);
      DrawCapitals(bmp, capitals);
   }

   private static void DrawCapitals(Bitmap bmp, List<Point> capitals)
   {
      var g = Graphics.FromImage(bmp);
      foreach (var capital in capitals)
         DrawCapital(ref g, capital);
      g.Dispose();
   }

   private static void DrawCapital(ref Graphics g, Point provinceCenter)
   {
      g.DrawRectangle(new(Color.Black, 1), provinceCenter.X - 2, provinceCenter.Y - 2, 4, 4);
      g.DrawRectangle(Pens.Yellow, provinceCenter.X - 1, provinceCenter.Y - 1, 2, 2);
   }

   public static void RedrawCapitals(Bitmap bmp, List<int> provinceIds)
   {
      List<Point> redrawList = [];
      foreach (var id in provinceIds)
         if (Globals.Capitals.TryGetValue(id, out var prov))
            redrawList.Add(Globals.Provinces[prov].Center);
      if (redrawList.Count > 0)
         DrawCapitals(bmp, redrawList);
   }
}
