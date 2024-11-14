﻿using System.Globalization;
using System.Text;

namespace Editor.Saving
{
   public static class SavingUtil
   {
      public static int MaxLineWidth = 125;
      public static bool SortIntLists = true;
      public static bool SameSpacePerInt = true;
      public static bool OneLinePerString = true;

      public static string GetYesNo(bool value)
      {
         return value ? "yes" : "no";
      }

      private static void AddFormattedList(string blockName, ICollection<string> strings, int tabCount, bool isNumber, ref StringBuilder sb)
      {
         AddTabs(tabCount, ref sb);

         sb.Append(blockName).Append(" = {\n");
         AddTabs(tabCount + 1, ref sb);
         var lineLength = 0;
         foreach (var i in strings)
         {
            if (lineLength > MaxLineWidth)
            {
               sb.Append('\n');
               AddTabs(tabCount + 1, ref sb);
               lineLength = 0;
            }

            if (isNumber && SameSpacePerInt)
            {
               sb.Append($"{i,4}").Append(' ');
               lineLength += 5;
            }
            else if (!isNumber && !OneLinePerString)
            {
               sb.AppendLine($"{i} ");
               AddTabs(tabCount + 1, ref sb);
            }
            else
            {
               sb.Append(i).Append(' ');
               lineLength += i.Length + 1;
            }
         }
         if (isNumber)
            sb.Append('\n');
         else if (!OneLinePerString)
            sb.Remove(sb.Length - 1, 1);
         else
            sb.Remove(sb.Length - 2, 2);
         AddTabs(tabCount, ref sb);
         sb.Append("}\n\n");
      }

      public static void AddFormattedIntList(string blockName, ICollection<int> ints, int tabCount, ref StringBuilder sb)
      {
         if (SortIntLists)
         {
            var intsList = ints.ToList();
            intsList.Sort();
            ints = intsList;
         }
         AddFormattedList(blockName, ints.Select(i => i.ToString()).ToList(), tabCount, true, ref sb);
      }

      public static void AddFormattedPointFList(string blockName, ICollection<PointF> floats, int tabCount, bool adjustHeight, ref StringBuilder sb)
      {
         List<string> strings = [];
         for (var i = 0; i < floats.ToList().Count; i++)
         {
            var f = floats.ToList()[i];
            strings.Add(f.X.ToString("F6", CultureInfo.InvariantCulture));
            if (adjustHeight)
               f.Y = Globals.MapHeight - f.Y;
            strings.Add(f.Y.ToString("F6", CultureInfo.InvariantCulture));
         }

         AddTabs(tabCount, ref sb);
         sb.AppendLine($"{blockName} = {{");
         var lineLength = 0;
         AddTabs(tabCount + 1, ref sb);
         foreach (var s in strings)
         {
            if (lineLength > MaxLineWidth)
            {
               sb.Append('\n');
               AddTabs(tabCount + 1, ref sb);
               lineLength = 0;
            }

            sb.Append($"{s,11}").Append(' ');
            lineLength += s.Length + 1;
         }

         sb.Append('\n');
         AddTabs(tabCount, ref sb);
         sb.AppendLine("}");
      }

      public static void AddFormattedStringListOnePerRow(string blockName, ICollection<string> strings, int tabCount,
         ref StringBuilder sb)
      {
         AddTabs(tabCount, ref sb);
         sb.AppendLine($"{blockName} = {{");
         foreach (var s in strings)
         {
            AddTabs(tabCount + 1, ref sb);
            sb.AppendLine(s);
         }
         AddTabs(tabCount, ref sb);
         sb.AppendLine("}");
      }

      public static void AddFormattedStringList(string blockName, ICollection<string> strings, int tabCount, ref StringBuilder sb)
      {
         AddFormattedList(blockName, strings, tabCount, false, ref sb);
      }

      public static void AddTabs(int tabCount, ref StringBuilder sb)
      {
         for (var i = 0; i < tabCount; i++) 
            sb.Append('\t');
      }

      public static void AddColor(int tabs, Color color, ref StringBuilder sb)
      {
         AddTabs(tabs, ref sb);
         sb.AppendLine($"color = {{ {color.R} {color.G} {color.B} }}");
      }

      public static void AddBool(int tabs, bool b, string boolName, ref StringBuilder sb)
      {
         AddTabs(tabs, ref sb);
         sb.AppendLine($"{boolName} = {GetYesNo(b)}");
      }

      public static void AddInt(int tabs, int b, string intName, ref StringBuilder sb)
      {
         AddTabs(tabs, ref sb);
         sb.AppendLine($"{intName} = {b}");
      }
   }
}