﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Editor;
using Editor.Helper;

public static class DebugMaps
{
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
         foreach (var prov in Data.Provinces)
         {
            if (Data.AdjacentProvinces.TryGetValue(prov.SelfPtr, out var province))
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
            Array.Copy(Data.BorderPixels, prov.BorderPtr, points, 0, prov.BorderCnt);
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

}