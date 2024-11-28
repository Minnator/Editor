﻿using System.Text;
using Editor.DataClasses.Misc;

namespace Editor.DataClasses.GameDataClasses;

public class HistoryEntry(Date date)
{
   public Date Date { get; set; } = date;
   public List<Effect> Effects { get; set; } = [];

   public void Apply(Province province)
   {
      foreach (var effect in Effects)
      {
         effect.ExecuteProvince(province);
      }
   }

   public override string ToString()
   {
      var sb = new StringBuilder();
      sb.AppendLine($"Date: {Date}");

      foreach (var effect in Effects)
      {
         sb.AppendLine(effect.ToString());
      }

      return sb.ToString();
   }
}

