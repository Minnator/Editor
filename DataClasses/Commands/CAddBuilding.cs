﻿using Editor.Commands;
using Editor.DataClasses.GameDataClasses;

namespace Editor.DataClasses.Commands
{
   public class CAddBuilding : ICommand
   {
      private readonly bool _add;
      private readonly string _building;
      private readonly List<Province> _provinces;
      public CAddBuilding(List<Province> provinces, bool add, string building, bool executeOnInit = true)
      {
         _provinces = provinces;
         _add = add;
         _building = building;

         if (executeOnInit)
            Execute();
      }


      public void Execute()
      {
         foreach (var province in _provinces) 
            province.SetAttribute(_building, _add ? "yes" : "no");
      }

      public void Undo()
      {
         foreach (var province in _provinces)
            province.SetAttribute(_building, _add ? "no" : "yes");
      }

      public void Redo()
      {
         Execute();
      }

      public string GetDescription()
      {
         return _provinces.Count == 1
            ? $"{(_add ? "Added" : "Removed")} {_building} from {_provinces[0].Id} ({_provinces[0].GetLocalisation()})"
            : $"{(_add ? "Added" : "Removed")} {_building} from [{_provinces.Count}] provinces";
      }
   }
}