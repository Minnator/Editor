﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Editor.DataClasses;

namespace Editor.Helper;

public static class MapHelper
{
   // ================================ Public Methods ================================
   // Draws the given Array of Points on the given Bitmap with the given Color
   public static Rectangle DrawOnMap(Rectangle rect, Point[] points, Color color, Bitmap bmp)
   {
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
   
   // Draws the border of the given province on the given Bitmap with the given Color
   public static Rectangle DrawProvinceBorder(int provincePtr, Color color, Bitmap bmp)
   {
      var province = Data.Provinces[provincePtr];
      var points = new Point[province.BorderCnt];
      Array.Copy(Data.BorderPixels, province.BorderPtr, points, 0, province.BorderCnt);
      return DrawOnMap(province.Bounds, points, color, bmp);
   }


   // ================================ Private Methods ================================
   private static Rectangle GetBoundingBox(ref Point[] points)
   {
      var minX = points.Min(p => p.X);
      var minY = points.Min(p => p.Y);
      var maxX = points.Max(p => p.X);
      var maxY = points.Max(p => p.Y);

      return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
   }

   // ------------------------------ 24bpp ------------------------------
   #region 24bpp Drawing
   // !!24bpp!! Draws the given Array of Points on the given Bitmap with the given Color in parallel
   public static void DrawPixels24BppParallel(Rectangle rect, Point[] points, Color color, Bitmap bmp)
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
   public static void DrawPixels24Bpp(Rectangle rect, Point[] points, Color color, Bitmap bmp)
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

            scan0[index] = b;       // Blue component
            scan0[index + 1] = g;   // Green component
            scan0[index + 2] = r;   // Red component
            scan0[index + 3] = a;   // Alpha component
         });
      }

      bmp.UnlockBits(bmpData);
   }
   #endregion

}
