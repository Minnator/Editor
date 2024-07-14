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

      public override string GetSpecificToolTip(int provinceId)
      {
         var node = TradeNodeHelper.GetTradeNodeByProvince(provinceId);
         if (Equals(node, TradeNode.Empty))
            return "TradeNode: <undefined>";
         var sb = new StringBuilder();
         sb.AppendLine($"TradeNode: {node.Name} ({Localisation.GetLoc(node.Name)})");
         sb.AppendLine($"Inland: {node.IsInland}");
         sb.AppendLine($"Outgoing: {node.Outgoing}");
         foreach (var outgoing in node.Outgoing)
            sb.Append($" {outgoing},");
         sb.AppendLine($"Incoming: {node.Incoming}");
         foreach (var incoming in node.Incoming)
            sb.Append($" {incoming},");
         return sb.ToString();
      }
   }
}