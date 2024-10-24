﻿using System.Collections.Generic;
using System.Drawing;
using Editor.Events;
using Editor.Helper;
using Editor.Interfaces;

namespace Editor.DataClasses.GameDataClasses;
#nullable enable
public class Region(string name) : IProvinceCollection
{
   private List<string> _areas = [];
   public string Name { get; } = name;
   public Rectangle Bounds { get; set; } = Rectangle.Empty;
   public List<string> Areas
   {
      get => _areas;
      init
      {
         foreach (var area in value) 
            AddRmvArea(area, true);
      }
   }

   public List<Monsoon> Monsoon { get; set; } = [];
   public string SuperRegion { get; set; } = string.Empty;
   public Color Color { get; set; }

   public Region(string name, List<string> areas) : this(name)
   {
      Areas = areas;
   }

   public Region(string name, List<string> areas, List<Monsoon> monsoon) : this(name, areas)
   {
      Monsoon = monsoon;
   }

   public void CalculateBounds()
   {
      List<Rectangle> areaBounds = [];
      foreach (var area in Areas)
      {
         if (Globals.Areas.TryGetValue(area, out var areaObj))
            areaBounds.Add(areaObj.Bounds);
      }
      Bounds = Geometry.GetBounds(areaBounds);
   }

   public void AddArea(string areaName)
   {
      AddRmvArea(areaName, true);
   }

   public void RemoveArea(string areaName)
   {
      AddRmvArea(areaName, false);
   }

   private void AddRmvArea(string areaName, bool add)
   {

      if (add)
      {
         if (!_areas.Contains(areaName))
            _areas.Add(areaName);
      }
      else
      {
         _areas.Remove(areaName);
      }
      if (Globals.State == State.Running)
         if (Globals.Areas.TryGetValue(areaName, out var area))
            foreach (var id in area.Provinces)
               ProvinceEventHandler.RaiseProvinceRegionAreasChanged(id, _areas, nameof(Areas));
      CalculateBounds();
   }

   public override bool Equals(object? obj)
   {
      if (obj is Region other)
         return Name == other.Name;
      return false;
   }

   public override int GetHashCode()
   {
      return Name.GetHashCode();
   }

   public int[] GetProvinceIds()
   {
      var provinces = new List<int>();
      foreach (var area in Areas)
         provinces.AddRange(Globals.Areas[area].GetProvinceIds());
      return provinces.ToArray();
   }

   public ICollection<Province> GetProvinces()
   {
      var provinces = new List<Province>();
      foreach (var area in Areas)
         provinces.AddRange(Globals.Areas[area].GetProvinces());
      return provinces;
   }

   public IProvinceCollection ScopeOut()
   {
      return Globals.SuperRegions[SuperRegion];
   }

   public List<IProvinceCollection> ScopeIn()
   {
      var areas = new List<IProvinceCollection>();
      foreach (var area in Areas)
      {
         areas.Add(Globals.Areas[area]);
      }
      return areas;
   }

}

public class Monsoon(string start, string end)
{
   public string Start { get; set; } = start;
   public string End { get; set; } = end;
}