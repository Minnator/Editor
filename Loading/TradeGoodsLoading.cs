﻿using System.Diagnostics;
using System.Text;
using Editor.DataClasses;
using Editor.DataClasses.GameDataClasses;
using Editor.Helper;

namespace Editor.Loading
{
   //=================================================================================================================
   // ALSO CONTAINS PRICE LOADING
   //=================================================================================================================

   public static class TradeGoodsLoading
   {

      public static void Load(ModProject project)
      {
         var sw = Stopwatch.StartNew();
         var files = FilesHelper.GetFilesFromModAndVanillaUniquely(
            project.ModPath, project.VanillaPath, "common", "tradegoods");

         foreach (var file in files)
         {
            ParseTradeGoodsFromFile(IO.ReadAllInUTF8(file));
         }

         sw.Stop();
         Globals.LoadingLog.WriteTimeStamp("Loading TradeGoods", sw.ElapsedMilliseconds);
      }

      private static void ParseTradeGoodsFromFile(string content)
      {
         Parsing.RemoveCommentFromMultilineString(content, out var removed);
         var elements = Parsing.GetElements(0, removed);

         foreach (var element in elements)
         {
            if (element is not Block block)
            {
               Globals.ErrorLog.Write($"Cant parse Tradegood: Element is not a block: {((Content)element)}");
               continue;
            }
            
            var color = block.GetBlockWithName("color");
            if (color is null)
            {
               Globals.ErrorLog.Write($"Color is missing in Tradegood: {block.Name}");
               continue;
            }

            var tradeGood = new TradeGood(block.Name, Parsing.ParseColorPercental(color.GetContent));
            Globals.TradeGoods.Add(tradeGood.Name, tradeGood);
         }
      }

   }
}