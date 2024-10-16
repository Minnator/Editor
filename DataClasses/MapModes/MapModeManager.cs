﻿using Editor.DataClasses.GameDataClasses;
using Editor.Helper;
using Editor.MapModes;

namespace Editor.DataClasses.MapModes;

public enum MapModeType
{
   None,
   Province,
   Area,
   Regions,
   SuperRegion,
   Continent,
   Development,
   CenterOfTrade,
   Autonomy,
   Fort,
   CultureGroup,
   Culture,
   Country,
   TradeGoods,
   TradeNode,
   TradeCompany,
   ColonialRegions,
   Religion,
   Devastation,
   Prosperity,
   Hre,
   ParliamentSeat,
   City,
   HasCapital,
   Diplomatic
}

public class MapModeManager
{
   private List<MapMode> MapModes { get; } = [];
   public MapMode CurrentMapMode { get; set; } = null!;
   public MapModeType CurrentMapModeType { get; set; }
   public ProvinceIdMapMode IdMapMode { get; set; } = null!;
   public bool PreviousLandOnly { get; set; }
   public bool RequireFullRedraw { get; set; } = true;

   public EventHandler<MapMode> MapModeChanged = delegate { };

   public MapModeManager()
   {
      InitializeAllMapModes();
   }

   public void InitializeAllMapModes()
   {
      MapModes.Add(new ProvinceMapMode());
      MapModes.Add(new AreaMapMode());
      MapModes.Add(new RegionsMapMode());
      MapModes.Add(new SuperRegionMapMode());
      MapModes.Add(new ContinentMapMode());
      MapModes.Add(new DevelopmentMapMode());
      MapModes.Add(new CenterOfTradeMapMode());
      MapModes.Add(new AutonomyMapMode());
      MapModes.Add(new FortMapMode());
      MapModes.Add(new CultureGroupMapMode());
      MapModes.Add(new CultureMapMode());
      MapModes.Add(new CountryMapMode());
      MapModes.Add(new TradeGoodsMapMode());
      MapModes.Add(new TradeNodeMapMode());
      MapModes.Add(new TradeCompanyMapMode());
      MapModes.Add(new ColonialRegionsMapMode());
      MapModes.Add(new ReligionMapMode());
      MapModes.Add(new DevastationMapMode());
      MapModes.Add(new ProsperityMapMode());
      MapModes.Add(new HreMapMode());
      MapModes.Add(new ParliamentSeatMapMode());
      MapModes.Add(new CityMapMode());
      MapModes.Add(new HasCapitalMapMode());
      MapModes.Add(new DiplomaticMapMode());



      // We set the default map mode to retrieve province colors

      IdMapMode = new ()
      {
         Bitmap = new(Globals.MapPath)
      };
   }
   

   public void RenderCurrent()
   {
      CurrentMapMode.RenderMapMode(CurrentMapMode.GetProvinceColor);
   }

   public List<MapMode> GetMapModes()
   {
      return MapModes;
   }

   public MapMode GetMapMode(MapModeType name)
   {
      return MapModes.Find(mode => mode.GetMapModeName() == name) ?? IdMapMode;
   }
   
   public void SetCurrentMapMode(MapModeType name)
   {
      // CAN be null
      // REMOVE old event handle form GlobalsEventhandler
      if (CurrentMapMode?.GetMapModeName() == name) 
         return; // no need to change map mode if it is already the same
      CurrentMapMode?.SetInactive();
      CurrentMapMode = GetMapMode(name); 
      CurrentMapMode.SetActive();
      CurrentMapMode.RenderMapMode(CurrentMapMode.GetProvinceColor);
      GC.Collect(); // We need to collect the garbage to free up memory but this is not ideal
      Globals.MapWindow.MapModeComboBox.SelectedItem = name.ToString();
      CurrentMapModeType = name;
      MapModeChanged(this, CurrentMapMode);
   }

   public int GetMapModeColor(Province p)
   {
      return CurrentMapMode.GetProvinceColor(p);
   }


}