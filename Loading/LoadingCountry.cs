﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Editor.DataClasses;
using Editor.DataClasses.GameDataClasses;
using Editor.Helper;

namespace Editor.Loading
{
   public static class CountryLoading
   {
      private static readonly Regex CountryRegex = new(@"(?<tag>[A-Z]{3})\s*=\s*""(?<path>[^""]+)""", RegexOptions.Compiled);

      public static void LoadCountries(ModProject project)
      {
         var sw = Stopwatch.StartNew();
         // Loads the country_tags file
         FilesHelper.GetFilesUniquelyAndCombineToOne(project.ModPath, project.VanillaPath, out var content, "common", "country_tags");

         Parsing.RemoveCommentFromMultilineString(ref content, out var removed);
         var matches = CountryRegex.Matches(removed);
         Dictionary<Tag, Country> countries = new(matches.Count);

         foreach (Match match in matches)
         {
            var tag = new Tag(match.Groups["tag"].Value);
            if (countries.ContainsKey(tag))
            {
               Globals.ErrorLog.Write($"Duplicate country tag: {tag}");
               continue;
            }
            countries.Add(tag, new(tag, match.Groups["path"].Value));
         }

         Globals.Countries = countries;

         sw.Stop();
         Globals.LoadingLog.WriteTimeStamp("Parsing Country Tags", sw.ElapsedMilliseconds);
         ParseCountryAttributes(project);

         // Load country history
         sw.Restart();
         LoadCountryHistories(project);
         sw.Stop();
         Globals.LoadingLog.WriteTimeStamp("Loading CountryHistories", sw.ElapsedMilliseconds);
      }

      private static void LoadCountryHistories(ModProject project)
      {
         var files = FilesHelper.GetFilesFromModAndVanillaUniquely(project.ModPath, project.VanillaPath, "history", "countries");

         Parallel.ForEach(files, new () { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 },file =>
         {
            Parsing.RemoveCommentFromMultilineString(IO.ReadAllInUTF8(file), out var removed);
            var elements = Parsing.GetElements(0, removed);

            foreach (var element in elements) 
               AnalyzeCountryStuff(element, Globals.Countries[new(Path.GetFileName(file)[..3])]);
         });
      }

      private static void AnalyzeCountryStuff(IElement element, Country country)
      {
         if (element is Block block)
         {
            ParseHistoryBlock(block, out var che);
            country.History.Add(che);
         }
         else
         {
            ParseCountryHistoryAttributes((Content)element, ref country);
         }
      }

      private static void ParseCountryHistoryAttributes(Content content, ref Country country)
      {
         foreach (var kvp in Parsing.GetKeyValueList(content.Value))
         {
            var val = Parsing.RemoveCommentFromLine(kvp.Value);
            switch (kvp.Key)
            {
               case "government":
                  country.Government = val;
                  break;
               case "religion":
                  country.Religion = val;
                  break;
               case "technology_group":
                  country.TechnologyGroup = val;
                  break;
               case "national_focus":
                  country.NationalFocus = Parsing.ManaFromString(val);
                  break;
               case "add_historical_rival":
               case "historical_rival":
                  country.HistoricalRivals.Add(val);
                  break;
               case "add_historical_friend":
               case "historical_friend":
                  country.HistoricalFriends.Add(val);
                  break;
               case "set_estate_privilege":
                  country.EstatePrivileges.Add(val);
                  break;
               case "religious_school":
                  country.ReligiousSchool = val;
                  break;
               case "add_harmonized_religion":
                  country.HarmonizedReligions.Add(val);
                  break;
               case "secondary_religion":
                  country.SecondaryReligion = val;
                  break;
               case "unit_type":
                  country.UnitType = val;
                  break;
               case "capital":
                  if (int.TryParse(val, out var value))
                     country.Capital = value;
                  else
                     Globals.ErrorLog.Write($"Invalid capital in {country.Tag}: {val}");
                  break;
               case "add_government_reform":
                  country.GovernmentReforms.Add(val);
                  break;
               case "add_accepted_culture":
                  country.AcceptedCultures.Add(val);
                  break;
               case "government_rank":
                  if (int.TryParse(val, out var rank))
                     country.GovernmentRank = rank;
                  else
                     Globals.ErrorLog.Write($"Invalid government rank in {country.Tag}: {val}");
                  break;
               case "primary_culture":
                  country.PrimaryCulture = val;
                  break;
               case "fixed_capital":
                  if (int.TryParse(val, out var capProv))
                     country.FixedCapital = capProv;
                  else
                     Globals.ErrorLog.Write($"Invalid fixed capital in {country.Tag}: {val}");
                  break;
               case "mercantilism":
                  if (int.TryParse(val, out var mercantilism))
                     country.Mercantilism = mercantilism;
                  else
                     Globals.ErrorLog.Write($"Invalid mercantilism in {country.Tag}: {val}");
                  break;
               case "add_army_tradition":
                  if (int.TryParse(val, out var armyTradition))
                     country.ArmyTradition = armyTradition;
                  else
                     Globals.ErrorLog.Write($"Invalid army tradition in {country.Tag}: {val}");
                  break;
               case "add_army_professionalism":
                  if (float.TryParse(val, out var armyProfessionalism))
                     country.ArmyProfessionalism = armyProfessionalism;
                  else
                     Globals.ErrorLog.Write($"Invalid army professionalism in {country.Tag}: {val}");
                  break;
               case "add_prestige":
                  if (float.TryParse(val, out var prestige))
                     country.Prestige = prestige;
                  else
                     Globals.ErrorLog.Write($"Invalid prestige in {country.Tag}: {val}");
                  break;
               case "unlock_cult":
                  country.UnlockedCults.Add(val);
                  break;
               case "elector":
                  country.IsElector = Parsing.YesNo(val);
                  break;
               case "add_truce_with":
                  //TODO
                  break;
               case "add_piety":
                  //TODO
                  break;
               default:
                  Globals.ErrorLog.Write($"Unknown key in toppers {country.Tag}: {kvp.Key}");
                  break;
            }
         }
      }

      private static void ParseHistoryBlock(Block block, out CountryHistoryEntry che)
      {
         che = null!;

         if (!DateTime.TryParse(block.Name, out var date))
         {
            if (!Parsing.ParseDynamicContent(block, out _))
               return;
         }

         che = new(date);
         AssignHistoryEntryAttributes(ref che, block.Blocks);
         AssignHistoryEntryContent(ref che, block.GetContentElements);
      }

      private static void AssignHistoryEntryAttributes(ref CountryHistoryEntry che, List<IElement> elements)
      {
         if (elements.Count == 0)
            return;

         foreach (var element in elements)
         {
            if (element is not Block block)
            {
               continue;
            }

            switch (block.Name)
            {
               case "monarch":
               case "heir":
               case "queen":
               case "monarch_consort":
                  Parsing.ParsePersonFromString(block.GetContentElements[0].Value, out var person);
                  che.Persons.Add(person);
                  break;
               case "leader":
                  Parsing.ParseLeaderFromString(block.GetContentElements[0].Value, out var leader);
                  che.Leaders.Add(leader);
                  break;
               default:
                  if (Parsing.ParseDynamicContent(block, out _))
                     break;
                  Globals.ErrorLog.Write($"Unknown block in history entry: {block.Name}");
                  break;
            }
         }
      }

      private static void AssignHistoryEntryContent(ref CountryHistoryEntry che, List<Content> element)
      {
         if (element.Count == 0)
            return;
         foreach (var content in element)
         {
            var kvp = Parsing.GetKeyValueList(content.Value);
            if (kvp.Count < 1)
            {
               Globals.ErrorLog.Write($"Invalid key value pair in history entry: {content.Value}");
               continue;
            }
            che.Effects.AddRange(kvp);
         }
      }

      #region CountryTags and non history data

      private static void ParseCountryAttributes(ModProject project)
      {
         var sw = Stopwatch.StartNew();

         Parallel.ForEach(Globals.Countries.Values, new () { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 }, country =>
         {
            FilesHelper.GetFileUniquely(project.ModPath, project.VanillaPath, out var content, "common", country.FileName);
            Parsing.RemoveCommentFromMultilineString(ref content, out var removed);
            var blocks = Parsing.GetElements(0, ref removed);

            AssignCountryAttributes(country, ref blocks);
         });

         sw.Stop();
         Globals.LoadingLog.WriteTimeStamp("CountryAttributes", sw.ElapsedMilliseconds);
      }

      private static void AssignCountryAttributes(Country country, ref List<IElement> blocks)
      {
         foreach (var element in blocks)
         {
            if (element is not Block block)
            {
               AssignCountryContent(country, (Content)element);
               continue;
            }

            switch (block.Name)
            {
               case "color":
                  if (block.Blocks.Count != 1)
                  {
                     Globals.ErrorLog.Write($"Invalid color block in {country.Tag} at [color]");
                     break;
                  }
                  country.Color = Parsing.ParseColor(((Content)block.Blocks[0]).Value);
                  break;
               case "revolutionary_colors":
                  if (block.Blocks.Count != 1)
                  {
                     Globals.ErrorLog.Write($"Invalid revolutionary_colors block in {country.Tag} at [revolutionary_colors]");
                     break;
                  }
                  country.RevolutionaryColor = Parsing.ParseColor(((Content)block.Blocks[0]).Value);
                  break;
               case "historical_idea_groups":
                  foreach (var idea in block.GetContentElements)
                     country.HistoricalIdeas.AddRange(Parsing.GetStringList(idea.Value));
                  break;
               case "historical_units":
                  foreach (var unit in block.GetContentElements)
                     country.HistoricalUnits.AddRange(Parsing.GetStringList(unit.Value));
                  break;
               case "monarch_names":
                  foreach (var name in block.GetContentElements)
                  {
                     Parsing.ParseMonarchNames(name.Value, out var monarchNames);
                     country.MonarchNames.AddRange(monarchNames);
                  }
                  break;
               case "ship_names":
                  foreach (var name in block.GetContentElements)
                     country.ShipNames.AddRange(Parsing.GetStringList(name.Value));
                  break;
               case "fleet_names":
                  foreach (var name in block.GetContentElements)
                     country.FleeTNames.AddRange(Parsing.GetStringList(name.Value));
                  break;
               case "army_names":
                  foreach (var name in block.GetContentElements)
                     country.ArmyNames.AddRange(Parsing.GetStringList(name.Value));
                  break;
               case "leader_names":
                  foreach (var name in block.GetContentElements)
                     country.LeaderNames.AddRange(Parsing.GetStringList(name.Value));
                  break;
               default:
                  Globals.ErrorLog.Write($"Unknown block in {country.Tag}: {block.Name}");
                  break;
            }
         }
      }

      private static void AssignCountryContent(Country country, Content element)
      {
         foreach (var kvp in Parsing.GetKeyValueList(element.Value))
         {
            switch (kvp.Key)
            {
               case "historical_council":
                  country.HistoricalCouncil = kvp.Value;
                  break;
               case "historical_score":
                  country.HistoricalScore = int.Parse(kvp.Value);
                  break;
               case "graphical_culture":
                  country.Gfx = kvp.Value;
                  break;
               case "random_nation_chance":
                  if (int.TryParse(kvp.Value, out var value) && value == 0)
                     country.CanBeRandomNation = false;
                  break;
               case "preferred_religion":
                  country.PreferredReligion = kvp.Value;
                  break;
               case "colonial_parent":
                  country.ColonialParent = new(kvp.Value);
                  break;
               case "special_unit_culture":
                  country.SpecialUnitCulture = kvp.Value;
                  break;
               default:
                  Globals.ErrorLog.Write($"Unknown key in {country.Tag}: {kvp.Key}");
                  break;
            }
         }
      }

      #endregion

   }
}