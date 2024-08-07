﻿using System.Text;
using Editor.DataClasses.GameDataClasses;
using Editor.Helper;

namespace Editor.DataClasses.MapModes
{
   public class TradeNodeMapMode : MapMode
   {
      public override string GetMapModeName()
      {
         return "Trade Nodes";
      }

      public override Color GetProvinceColor(int id)
      {
         var node = TradeNodeHelper.GetTradeNodeByProvince(id);
         if (Equals(node, TradeNode.Empty))
            return Color.DimGray;
         return node.Color;
      }

      public override void RenderMapMode(Func<int, Color> method)
      {
         base.RenderMapMode(method);
         BitMapHelper.WriteOnProvince(GetCoTLevel, Globals.MapModeManager.ShareLiveBitmap);
      }

      private string GetCoTLevel(int provinceId)
      {
         if (Globals.Provinces.TryGetValue(provinceId, out var prov) && prov.CenterOfTrade > 0)
            return prov.CenterOfTrade.ToString();
         return string.Empty;
      }

      public override string GetSpecificToolTip(int provinceId)
      {
         var node = TradeNodeHelper.GetTradeNodeByProvince(provinceId);
         if (Equals(node, TradeNode.Empty))
            return "TradeNode: <undefined>";
         var sb = new StringBuilder();
         sb.AppendLine($"TradeNode: {node.Name} ({Localisation.GetLoc(node.Name)})");
         sb.AppendLine($"Inland: {node.IsInland}");
         if (Globals.Provinces.TryGetValue(provinceId, out var prov))
            if (prov.CenterOfTrade > 0)
               sb.AppendLine($"Center of Trade: {prov.CenterOfTrade}");
         sb.AppendLine($"Outgoing: ");
         sb.Append("\t");
         if (node.Outgoing.Count > 0)
            foreach (var outgoing in node.Outgoing)
               sb.Append($" {outgoing},");
         sb.Remove(sb.Length - 1, 1);
         sb.AppendLine();
         sb.AppendLine($"Incoming: ");
         sb.Append("\t");
         if (node.Incoming.Count > 0)
            foreach (var incoming in node.Incoming)
               sb.Append($" {incoming},");
         sb.Remove(sb.Length - 1, 1);
         return sb.ToString();
      }
   }
}