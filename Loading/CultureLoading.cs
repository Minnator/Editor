﻿using System.Collections.Generic;
using System.Diagnostics;
using Editor.DataClasses;
using Editor.Helper;

namespace Editor.Loading;

public static class CultureLoading
{

   public static void LoadCultures(string modPath, string vanillaPath, out List<CultureGroup> groups, ref Log loadingLog, ref Log errorLog)
   {
      var sw = Stopwatch.StartNew();
      FilesHelper.GetFilesUniquelyAndCombineToOne(modPath, vanillaPath, out var culturesContent, "common", "cultures");

      var blocks = Parsing.GetNestedElementsIterative(0, ref culturesContent);

      GetCultureGroups(ref blocks, out groups, ref errorLog);

      sw.Stop();
      loadingLog.WriteTimeStamp("Parsing cultures", sw.ElapsedMilliseconds);
      sw.Restart();
      AnalyzeCultures(ref groups, ref errorLog);
      sw.Stop();
      loadingLog.WriteTimeStamp("Analyzing cultures", sw.ElapsedMilliseconds);
   }

   private static void GetCultureGroups(ref List<Block> blocks, out List<CultureGroup> cultureGroups, ref Log errorLog)
   {
      cultureGroups = [];
      foreach (var block in blocks)
      {
         var group = new CultureGroup(block.Name);
         var contents = block.GetContentElements;
         var cultures = block.GetBlockElements;

         foreach (var cult in cultures)
         {
            if (cult.Name.Equals("country") || cult.Name.Equals("province"))
            {
               continue;
            }
            if (cult.Name.Equals("male_names") || cult.Name.Equals("female_names") || cult.Name.Equals("dynasty_names"))
            {
               SetCultureGroupNames(ref group, cult, ref errorLog);
            }
            else
            {
               var culture = new Culture(cult.Name);
               SetCultureAttributes(ref culture, cult.GetBlockElements, ref errorLog);
               SetCultureContent(ref culture, cult.GetContentElements, ref errorLog);
               group.Cultures.Add(culture);
            }
         }

         SetCultureGroupAttributes(ref group, contents, ref errorLog);
         cultureGroups.Add(group);
      }
   }

   private static void SetCultureContent(ref Culture culture, List<Content> cultGetContentElements, ref Log errorLog)
   {
      if (cultGetContentElements.Count == 0)
         return;

      foreach (var content in cultGetContentElements)
      {
         var kvp = Parsing.GetKeyValueList(content.Value);

         foreach (var item in kvp)
         {
            if (item.Key.Equals("primary"))
               culture.Primaries.Add(item.Value);
         }
      }
   }

   private static void SetCultureGroupNames(ref CultureGroup group, Block block, ref Log errorLog)
   {
      if (block.Blocks.Count == 0)
         return;
      var content = ((Content)block.Blocks[0]).Value;
      switch (block.Name)
      {
         case "male_names":
            group.MaleNames = [.. Parsing.GetStringList(content)];
            break;
         case "female_names":
            group.FemaleNames = [.. Parsing.GetStringList(content)];
            break;
         case "dynasty_names":
            group.DynastyNames = [.. Parsing.GetStringList(content)];
            break;
      }
   }

   private static void SetCultureGroupAttributes(ref CultureGroup group, List<Content> contents, ref Log errorLog)
   {
      foreach (var content in contents)
      {
         Parsing.RemoveCommentFromMultilineString(content.Value);
         var kvps = Parsing.GetKeyValueList(content.Value);

         foreach (var kvp in kvps)
         {
            switch (kvp.Key)
            {
               case "graphical_culture":
                  group.Gfx = kvp.Value;
                  break;
               case "second_graphical_culture":
                  group.SecondGfx = kvp.Value;
                  break;
               case "country":
                  group.CountryModifiers = Parsing.GetKeyValueList(kvp.Value);
                  break;
               case "province":
                  group.ProvinceModifiers = Parsing.GetKeyValueList(kvp.Value);
                  break;
               default:
                  errorLog.Write($"Unknown Group in a culture group file:{kvp.Key}");
                  break;
            }
         }
      }
   }

   private static void SetCultureAttributes(ref Culture culture, List<Block> blocks, ref Log errorLog)
   {
      foreach (var block in blocks)
      {
         if (block.Blocks.Count == 0)
            continue;
         var content = ((Content)block.Blocks[0]).Value;
         Parsing.RemoveCommentFromMultilineString(ref content);
         switch (block.Name.ToLower())
         {
            case "male_names":
               culture.MaleNames = [.. Parsing.GetStringList(ref content)];
               break;
            case "female_names":
               culture.FemaleNames = [.. Parsing.GetStringList(ref content)];
               break;
            case "dynasty_names":
               culture.DynastyNames = [.. Parsing.GetStringList(ref content)];
               break;
            case "country":
               culture.CountryModifiers = Parsing.GetKeyValueList(ref content);
               break;
            case "province":
               culture.ProvinceModifiers = Parsing.GetKeyValueList(ref content);
               break;
            default:
               errorLog.Write($"Unknown Group in a cultures file:{block.Name}");
               break;
         }
      }
   }

   private static void AnalyzeCultures(ref List<CultureGroup> groups, ref Log errorLog)
   {
      HashSet<string> cultureNames = [];
      HashSet<string> cultureGroupNames = [];

      foreach (var group in groups)
      {
         if (!cultureGroupNames.Add(group.Name))
            errorLog.Write($"Duplicate culture group name: {group.Name}");

         foreach (var culture in group.Cultures)
         {
            if (!cultureNames.Add(culture.Name))
               errorLog.Write($"Duplicate culture name: {culture.Name}");

            HashSet<string> duplicateNames = [];
            List<string> nonUniqueNames = [];
            foreach (var male in culture.MaleNames)
            {
               if (!duplicateNames.Add(male))
                  nonUniqueNames.Add(male);
            }
            foreach (var female in culture.FemaleNames)
            {
               if (!duplicateNames.Add(female))
                  nonUniqueNames.Add(female);
            }
            foreach (var dynasty in culture.DynastyNames)
            {
               if (!duplicateNames.Add(dynasty))
                  nonUniqueNames.Add(dynasty);
            }
            if (nonUniqueNames.Count > 0)
               errorLog.Write($"Duplicate names in culture {culture.Name}: {DebugPrints.GetListAsString(nonUniqueNames)}");
         }
      }
   }
}