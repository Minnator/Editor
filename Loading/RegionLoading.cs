﻿using Editor.DataClasses.GameDataClasses;
using Editor.Helper;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Region = Editor.DataClasses.GameDataClasses.Region;

namespace Editor.Loading;

public static class RegionLoading
{
   private static string _pattern =
      @"(?<regionName>[A-Za-z_]+)\s*=\s*{\s*areas\s*=\s*{\s*(?<areas>(?:\s*[A-Za-z_]+\s*)+)\s*}\s*(?<monsoons>(?:monsoon\s*=\s*{\s*(?:\s*[0-9.]+\s*)+\s*}\s*)*)}";

   public static void Load(string folder, ColorProviderRgb colorProvider)
   {
      var sw = new Stopwatch();
      sw.Start();
      var path = Path.Combine(folder, "map", "region.txt");
      var newContent = IO.ReadAllLinesInUTF8(path);
      List<string> regionContent = [];
      Dictionary<string, Region> regionDictionary = [];

      // Filtering Comments and optional that are not important to the regions themselves
      var sb = new StringBuilder();

      foreach (var line in newContent)
      {
         if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
            continue;
         sb.AppendLine(Parsing.RemoveCommentFromLine(line));
      }

      var content = sb.ToString();
      var matches = Regex.Matches(content, _pattern, RegexOptions.Multiline);

      foreach (Match match in matches)
      {
         var regionName = match.Groups["regionName"].Value;
         var areas = Parsing.GetStringList(match.Groups["areas"].Value);
         var monsoons = new List<Monsoon>();

         foreach (Capture monsoon in match.Groups["monsoons"].Captures)
         {
            var monsoonMatches = Regex.Matches(
               monsoon.Value, @"monsoon\s*=\s*{\s*(?<start>[0-9.]+)\s*(?<end>[0-9.]+)\s*}");

            foreach (Match monsoonMatch in monsoonMatches) 
               monsoons.Add(new Monsoon(monsoonMatch.Groups["start"].Value, monsoonMatch.Groups["end"].Value));
         }
         var region = new Region(regionName, areas, monsoons)
         {
            Color = colorProvider.GetRandomColor()
         };
         regionDictionary.Add(regionName, region);

         foreach (var area in areas)
         {
            if (Globals.Areas.TryGetValue(area, out var ar))
               ar.Region = regionName;
         }
         
      }

      Globals.Regions = regionDictionary;
      sw.Stop();
      Globals.LoadingLog.WriteTimeStamp("Parsing regions", sw.ElapsedMilliseconds);
   }
}