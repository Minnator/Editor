﻿using Editor;
using Editor.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Editor.Interfaces;
using static System.Net.Mime.MediaTypeNames;

public static class DebugMaps
{
   public static void MapModeDrawing()
   {
      var sw = Stopwatch.StartNew();

      var bmp = new Bitmap(Globals.MapModeManager.GetMapMode("Provinces").Bitmap);
      BitMapHelper.WriteOnProvince(GetProvinceIDString, bmp);
      bmp.Save("C:\\Users\\david\\Downloads\\areas.png", ImageFormat.Png);
      sw.Stop();
      Debug.WriteLine($"MapModeDrawing: {sw.ElapsedMilliseconds} ms");
   }

   private static string GetProvinceIDString(int id)
   {
      return id.ToString();
   }

   public static void MapModeDrawing3()
   {
      List<IProvinceCollection> areas = [.. Globals.Areas.Values];
      var sw = Stopwatch.StartNew();

      var bmp = BitMapHelper.GenerateBitmapFromProvinceCollection(areas);

      MapDrawHelper.DrawAllProvinceBorders(bmp, Color.Black);
      bmp.Save("C:\\Users\\david\\Downloads\\areas12.png", ImageFormat.Png);
      sw.Stop();
      Debug.WriteLine($"MapModeDrawing: {sw.ElapsedMilliseconds} ms");
      DrawAreasOnMap();
      Debug.WriteLine($"----------------------------------------------");
      return;
      Test();
   }



   public static void DrawAreasOnMap()
   {
      var sw = Stopwatch.StartNew();

      var bmp = BitMapHelper.GenerateBitmapFromProvinces(GetColorArea);
      
      MapDrawHelper.DrawAllProvinceBorders(bmp, Color.Black);
      bmp.Save("C:\\Users\\david\\Downloads\\areas.png", ImageFormat.Png);
      sw.Stop();
      Debug.WriteLine($"DrawAreasOnMap: {sw.ElapsedMilliseconds} ms");
   }
   public static void MapModeDrawing2()
   {
      var sw = Stopwatch.StartNew();

      var bmp = new Bitmap(Globals.MapWidth, Globals.MapHeight);
      foreach (var province in Globals.Provinces.Values)
      {
         MapDrawHelper.DrawProvince(province.Id, province.Color, bmp);
      }
      MapDrawHelper.DrawAllProvinceBorders(bmp, Color.Black);

      bmp.Save("C:\\Users\\david\\Downloads\\areas.png", ImageFormat.Png);
      sw.Stop();
      Debug.WriteLine($"MapModeDrawing2: {sw.ElapsedMilliseconds} ms");
   }

   public static Color GetColorArea(int id)
   {
      if (Globals.Provinces.TryGetValue(id, out var prov))
      {
         if (Globals.Areas.TryGetValue(prov.Area, out var area))
         {
            return area.Color;
         }
      }
      return Color.Black;
   }

   public static void Test()
   {
      var sw = Stopwatch.StartNew();
      var provincePixels = Globals.Provinces.Values.SelectMany(province =>
      {
         var points = new Point[province.PixelCnt];
         Array.Copy(Globals.Pixels, province.PixelPtr, points, 0, province.PixelCnt);
         return points;
      }).ToArray();
      
      sw.Stop();
      Debug.WriteLine($"Test: {sw.ElapsedMilliseconds} ms {provincePixels.Length}");
      var ids = Globals.Provinces.Values.Select(province => province.Id).ToArray();
      sw.Restart();
      MapDrawHelper.GetAllPixelPoints(ids, out var points);
      sw.Stop();
      Debug.WriteLine($"Test2: {sw.ElapsedMilliseconds} ms {points.Length}");
   }



   public static void DrawAreasOnMap2()
   {
      var sw = Stopwatch.StartNew();
      var bmp = new Bitmap(Globals.MapWidth, Globals.MapHeight, PixelFormat.Format32bppRgb);
      using var g = Graphics.FromImage(bmp);
      var rand = new Random();

      foreach (var area in Globals.Areas.Values)
      {
         var color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
         for (int i = 0; i < area.Provinces.Length; i++)
         {
            var prov = Globals.Provinces[area.Provinces[i]];
            var points = new Point[prov.BorderCnt];
            Array.Copy(Globals.BorderPixels, prov.BorderPtr, points, 0, prov.BorderCnt);
            g.FillPolygon(new SolidBrush(color), points);
         }
      }

      sw.Stop();
      Debug.WriteLine($"DrawAreasOnMap: {sw.ElapsedMilliseconds} ms");

      bmp.Save("C:\\Users\\david\\Downloads\\areas.png", ImageFormat.Png);
   }








   public static unsafe void DrawAllBorder(ConcurrentDictionary<Color, List<Point>> points, Size size, string saveTo)
   {
      // Create a new Bitmap with specified size
      using var bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);

      // Lock the bitmap data
      BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

      try
      {
         // Pointer to the start of bitmap data
         byte* scan0 = (byte*)bmpData.Scan0.ToPointer();

         // Parallel processing of points
         Parallel.ForEach(points, kvp =>
         {
            Color color = kvp.Key;
            List<Point> pointList = kvp.Value;

            foreach (var point in pointList)
            {
               int index = point.Y * bmpData.Stride + point.X * 3; // Calculate pixel index

               // Set pixel color at calculated index
               scan0[index + 2] = color.R;   // Red component
               scan0[index + 1] = color.G;   // Green component
               scan0[index] = color.B;       // Blue component
            }
         });
      }
      finally
      {
         // Unlock the bitmap data
         bmp.UnlockBits(bmpData);
      }

      // Save the bitmap
      bmp.Save(saveTo, ImageFormat.Bmp);
   }

   public static void Bench()
   {
      var sw = new Stopwatch();
      var milisecons = new List<long>();

      for (int i = 0; i < 100; i++)
      {
         sw.Restart();
         sw.Stop();
         milisecons.Add(sw.ElapsedMilliseconds);
      }

      Debug.WriteLine($"Average time: {milisecons.Average()} ms");
      Debug.WriteLine($"Max time: {milisecons.Max()} ms");
      Debug.WriteLine($"Min time: {milisecons.Min()} ms");
      Debug.WriteLine("-----------------------------------------------------------");
      Debug.WriteLine("Total time: " + milisecons.Sum() + " ms");
   }

   public static void DrawAdjacencyNumbers(Bitmap bmp1)
   {
      var bmp = new Bitmap(bmp1);
      byte col = 0;

      using var g = Graphics.FromImage(bmp);

      unsafe
      {

         // Draw the adjacency numbers on the provinces
         foreach (var prov in Globals.Provinces.Values)
         {
            if (Globals.AdjacentProvinces.TryGetValue(prov.Id, out var province))
            {
               var str = $"{province.Length}";
               var font = new Font("Arial", 8);
               var size = g.MeasureString(str, font);
               var pointS = new Point(prov.Center.X - (int)size.Width / 2, prov.Center.Y - (int)size.Height / 2);

               g.DrawString(str, font, Brushes.Black, pointS);
            }
            if (prov.BorderCnt < 4)
               continue;
            var points = new Point[prov.BorderCnt];
            Array.Copy(Globals.BorderPixels, prov.BorderPtr, points, 0, prov.BorderCnt);
            var bmpData = bmp.LockBits(prov.Bounds, ImageLockMode.ReadWrite, bmp.PixelFormat);
            var scan0 = (byte*)bmpData.Scan0.ToPointer();
            foreach (var point in points)
            {
               var index = (point.Y - prov.Bounds.Y) * bmpData.Stride + (point.X - prov.Bounds.X) * 4;

               scan0[index] = col;       // Blue component
               scan0[index + 1] = col;   // Green component
               scan0[index + 2] = col;   // Red component
            }
            bmp.UnlockBits(bmpData);
         }
      }

      bmp.Save("C:\\Users\\david\\Downloads\\adjacency.png", ImageFormat.Png);
      bmp.Dispose();
   }

   public static void PrintProvinceTypeMap()
   {
      var bmp = BitMapHelper.GenerateBitmapFromProvinces(id =>
      {
         if (Globals.Provinces.TryGetValue(id, out var prov))
         {
            if (Globals.LandProvinces.Contains(prov.Id))
               return Color.Green;
            if (Globals.SeaProvinces.Contains(prov.Id))
               return Color.Blue;
            if (Globals.LakeProvinces.Contains(prov.Id))
               return Color.LightBlue;
            if (Globals.CoastalProvinces.Contains(prov.Id))
               return Color.Yellow;
         }
         return Color.Black;
      });

      bmp.Save("C:\\Users\\david\\Downloads\\provinceTypeMap.png", ImageFormat.Png);
      bmp.Dispose();
   }

   public static void AreasToMap()
   {
      Dictionary<string, Color> color = [];
      var rand = new Random();
      
      foreach (var area in Globals.Areas.Values)
         color.Add(area.Name, Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));

      var bmp = BitMapHelper.GenerateBitmapFromProvinces(id =>
      {
         if (Globals.Provinces.TryGetValue(id, out var prov))
         {
            if (Globals.Areas.TryGetValue(prov.Area, out var area))
               return color[area.Name];
         }
         return Color.Black;
      });

      bmp.Save("C:\\Users\\david\\Downloads\\areas.png", ImageFormat.Png);
      bmp.Dispose();
   }
}