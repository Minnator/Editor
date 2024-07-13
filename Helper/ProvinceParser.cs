﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Editor.DataClasses;

namespace Editor.Helper;

public static class ProvinceParser
{
   private const string ID_FROM_FILE_NAME_PATTERN = @"(\d+)\s*-?";
   private const string DATE_PATTERN = @"\d{1,4}\.\d{1,2}\.\d{1,2}";
   private const string ATTRIBUTE_PATTERN = "(?<key>\\w+)\\s*=\\s*(?<value>\"[^\"]*\"|[\\w-]+)";

   private const string MULTILINE_ATTRIBUTE_PATTERN =
      "(?<name>[A-Za-z_.0-9]+)\\s*=\\s*\\{\\s*(?<pairs>(?:\\s*[A-Za-z_.0-9]+\\s*=\\s*[^}\\s]+(?:\\s*\\n?)*)*)\\s*\\}\\s*(?<comment>#.*)?";

   private static readonly Regex DateRegex = new (DATE_PATTERN, RegexOptions.Compiled);
   private static readonly Regex IdRegex = new (ID_FROM_FILE_NAME_PATTERN, RegexOptions.Compiled);
   private static readonly Regex AttributeRegex = new (ATTRIBUTE_PATTERN, RegexOptions.Compiled);
   private static readonly Regex MultilineAttributeRegex = new (MULTILINE_ATTRIBUTE_PATTERN, RegexOptions.Compiled);



   public static void ParseAllUniqueProvinces(string modFolder, string vanillaFolder)
   {
      var sw = Stopwatch.StartNew();
      // Get all unique province files from mod and vanilla
      var files = FilesHelper.GetFilesFromModAndVanillaUniquely(modFolder, vanillaFolder, "history", "provinces");
      // Get All nested Blocks and Attributes from the files
      foreach (var file in files)
      {
         ProcessProvinceFile(file);
      }
      sw.Stop();
      Globals.LoadingLog.WriteTimeStamp("Parsing provinces", sw.ElapsedMilliseconds);
   }

   private static void ProcessProvinceFile(string path)
   {
      var match = IdRegex.Match(Path.GetFileName(path));
      if (!match.Success || !int.TryParse(match.Groups[1].Value, out var id))
      {
         Globals.ErrorLog.Write($"Could not parse province id from file name: {path}\nCould not match \'<number> <.*>\'");
         return;
      }

      if (!Globals.Provinces.TryGetValue(id, out var province))
      {
         Globals.ErrorLog.Write($"Could not find province with id {id}");
         return;
      }

      var fileContent = IO.ReadAllInUTF8(path);
      var blocks = Parsing.GetNestedElementsIterative(0, ref fileContent);

      foreach (var block in blocks)
      {
         if (block is Content content)
         {
            ParseProvinceContentBlock(ref province, content);
         }
         else
         {
            ParseProvinceBlockBlock(ref province, (Block)block);
         }
      }

   }

   private static void ParseProvinceContentBlock(ref Province province, Content content)
   {
      var attributes = Parsing.GetKeyValueList(content.Value);
      AssignAttributesToProvince(attributes, ref province);
   }

   private static void ParseProvinceBlockBlock(ref Province province, Block block)
   {
      if (!DateTime.TryParse(block.Name, out var date))
      {
         switch (block.Name.ToLower())
         {
            case "latent_trade_goods":
               var ltg = Parsing.GetLatentTradeGood(block.GetContentElements[0]);
               province.LatentTradeGood = ltg;
               return;
            default:
               Globals.ErrorLog.Write($"Could not parse date: {block.Name}");
               return;
         }
      }

      var che = new HistoryEntry(date);

      foreach (var element in block.Blocks)
      {
         if (element is Content content)
         {
            AddEffectsToHistory(ref che, content);
         }
         else if (element is Block subBlock && subBlock.HasOnlyContent)
         {
            var ce = EffectFactory.CreateComplexEffect(subBlock.Name, EffectValueType.Complex);
            if (subBlock.Blocks.Count == 0)
               AddEffectsToComplexEffect(ref ce, string.Empty);
            else
               AddEffectsToComplexEffect(ref ce, subBlock.GetContentElements[0].Value);

            che.Effects.Add(ce);
         }
      }

      province.History.Add(che);
   }

   private static void AddEffectsToComplexEffect(ref ComplexEffect ce, string content)
   {
      var attributes = Parsing.GetKeyValueList(content);
      foreach (var element in attributes)
      {
         var type = EffectValueType.String;
         if (int.TryParse(element.Value, out _))
            type = EffectValueType.Int;
         else if (float.TryParse(element.Value, out _))
            type = EffectValueType.Float;
         ce.Effects.Add(EffectFactory.CreateSimpleEffect(element.Key, element.Value, type));
      }
   }

   private static void AddEffectsToHistory(ref HistoryEntry che, Content content)
   {
      var attributes = Parsing.GetKeyValueList(content.Value);
      foreach (var element in attributes)
      {
         var type = EffectValueType.String;
         if (int.TryParse(element.Value, out _))
            type = EffectValueType.Int;
         else if (float.TryParse(element.Value, out _))
            type = EffectValueType.Float;
         che.Effects.Add(EffectFactory.CreateSimpleEffect(element.Key, element.Value, type));
      }
   }

   private static void AssignAttributesToProvince(List<KeyValuePair<string, string>> attributes, ref Province prov)
   {
      foreach (var att in attributes)
      {
         prov.SetAttribute(att.Key, att.Value);
      }
   }

}

public class AttributeParsingException(string message) : Exception(message);