﻿using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Editor.DataClasses.GameDataClasses;
using Editor.Helper;

namespace Editor.Loading;

public static class SuperRegionLoading
{
   private const string MAIN_PATTER = @"(?<name>[a-zA-Z_]*)\s*=\s*{\s*(?<regions>[\w\s]+)\s*\s*}";


   public static void Load()
   {
      var sw = Stopwatch.StartNew();
      if (!FilesHelper.GetModOrVanillaPath(out var path, "map", "superregion.txt"))
      {
         Globals.ErrorLog.Write("Error: superregion.txt not found!");
         return;
      }
      var newContent = IO.ReadAllLinesInUTF8(path);
      var sb = new StringBuilder();

      foreach (var line in newContent)
         sb.Append(Parsing.RemoveCommentFromLine(line));

      var matches = Regex.Matches(sb.ToString(), MAIN_PATTER, RegexOptions.Multiline);
      foreach (Match match in matches)
      {
         var superRegionName = match.Groups["name"].Value;
         var regions = Parsing.GetStringList(match.Groups["regions"].Value);

         var sRegion = new SuperRegion(superRegionName, regions)
         {
            Color = Globals.ColorProvider.GetRandomColor()
         };
         sRegion.CalculateBounds();
         Globals.AddSuperRegion(sRegion);

         foreach (var region in regions)
            if (Globals.Regions.TryGetValue(region, out var reg)) 
               reg.SuperRegion = superRegionName;
         
      }

      sw.Stop();
      Globals.LoadingLog.WriteTimeStamp("Parsing Super Regions", sw.ElapsedMilliseconds);
   }
}
