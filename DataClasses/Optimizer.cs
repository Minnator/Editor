﻿using System.Collections.Concurrent;
using System.Diagnostics;
using Editor.DataClasses.GameDataClasses;
using Editor.Helper;

namespace Editor.DataClasses;

public static class Optimizer
{
   // Optimizes the provinces by copying the pixels and borders to one large array each and only saving pointers in the provinces
   // to where their points start and end. Also calculates the bounds of the provinces.
   // This allows for duplicate points in the BorderPixels array but increases performance.
   public static void OptimizeProvinces(Province[] provinces, ConcurrentDictionary<Color, List<Point>> colorToProvId, ConcurrentDictionary<Color, List<Point>> colorToBorder, int pixelCount)
   {
      var sw = new Stopwatch();
      sw.Start();
      var pixels = new Point[pixelCount];
      var borders = new Point[colorToBorder.Values.Sum(list => list.Count)];
      var dic = new Dictionary<Color, int>(provinces.Length);
      var provs = new Dictionary<int, Province>(provinces.Length);

      var pixelPtr = 0;
      var borderPtr = 0;
      
      foreach (var province in provinces)
      {
         var color = Color.FromArgb(province.Color.R, province.Color.G, province.Color.B);
         dic[color] = province.Id;


         //copy the pixels of the province to the pixel array
         if (!colorToProvId.ContainsKey(color))
            continue;
         colorToProvId[color].CopyTo(pixels, pixelPtr);
         province.PixelPtr = pixelPtr;
         province.PixelCnt = colorToProvId[color].Count;
         pixelPtr += province.PixelCnt;

         //copy the borders of the province to the border array
         colorToBorder[color].CopyTo(borders, borderPtr);
         province.BorderPtr = borderPtr;
         province.BorderCnt = colorToBorder[color].Count;
         borderPtr += province.BorderCnt;

         //calculate the bounds of the provinces and set the center
         province.Bounds = Geometry.GetBounds([.. colorToBorder[color]]);
         province.Center = new Point(province.Bounds.X + province.Bounds.Width / 2, province.Bounds.Y + province.Bounds.Height / 2);

         // add the province to the dictionary
         provs.Add(province.Id, province);
      }

      sw.Stop();
      //Debug.WriteLine($"OptimizeProvinces took {sw.ElapsedMilliseconds}ms");
      Globals.LoadingLog.WriteTimeStamp("OptimizeProvinces", sw.ElapsedMilliseconds);
      //var elapsed = sw.ElapsedMilliseconds;
      //Debug.WriteLine($"Per Province Cost: {elapsed / (float)provinces.Length * 1000} µs");

      // Set the optimized data to the Globals class
      Globals.BorderPixels = borders;
      Globals.Pixels = pixels;
      Globals.Provinces = provs;
      Globals.ColorToProvId = dic;

      // Free up memory from the ConcurrentDictionaries
      colorToBorder.Clear();
      colorToProvId.Clear();
   }

   public static void OptimizeAdjacencies(ConcurrentDictionary<Color, HashSet<Color>> colorToAdj)
   {
      var sw = new Stopwatch();
      sw.Start();
      var adjacencyList = new Dictionary<int, int[]>(Globals.Provinces.Count);

      foreach (var kvp in colorToAdj) 
         adjacencyList.Add(Globals.ColorToProvId[kvp.Key], kvp.Value.Select(color => Globals.ColorToProvId[color]).ToArray());

      sw.Stop();
      //Debug.WriteLine($"Adjacency calculation took {sw.ElapsedMilliseconds}ms");
      Globals.LoadingLog.WriteTimeStamp("Adjacency optimization", sw.ElapsedMilliseconds); Globals.AdjacentProvinces = adjacencyList;

      // Free up memory from the ConcurrentDictionary
      colorToAdj.Clear();
   }

}