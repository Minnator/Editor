﻿using System.Security.Policy;
using Editor.Commands;
using Editor.DataClasses;
using Editor.DataClasses.GameDataClasses;
using Editor.DataClasses.MapModes;
using Editor.DataClasses.Settings;
using Editor.Forms;
using Editor.Helper;
using Editor.Loading;
using Region = Editor.DataClasses.GameDataClasses.Region;

namespace Editor;

#region enums
public enum CommandHistoryType
{
   SimpleSelection,
   ComplexSelection,
   Action
}
public enum State
{
   Loading,
   Running,
   Initializing,
   LoadingHistory,
   Waiting
}
public enum Mana
{
   ADM,
   DIP,
   MIL,
   NONE
}
public enum MapModeRendering
{
   Live,
   LiveBackground,
   Cached,
}
public enum EditingStatus
{
   LoadingInterface,
   Idle,
   Saving,
   Interrupted
}
public enum Language
{
   english,
   french,
   german,
   spanish,
}
public enum StripesDirection
{
   Horizontal,
   Vertical,
   DiagonalLbRt,
   DiagonalLtRb,
   Dotted,
   Pluses
}


#endregion

//contains all required and used data across the application and instances of forms.
public static class Globals
{
   public static string VanillaPath = string.Empty;
   public static string ModPath = string.Empty;

   public static Language Language
   {
      get => _language;
      set
      {
         _language = value;
         LocalisationLoading.Load();
      }
   }


   #region LoadingScreen
   public static event EventHandler<int> LoadingStageChanged = delegate { };

   public static int LoadingStage
   {
      get => _loadingStage;
      set
      {
         _loadingStage = value;
         LoadingStageChanged?.Invoke(null, _loadingStage);
      }
   }
   private static int _loadingStage = 0;
   public const int LOADING_STAGES = 27; // Needs to be increased when adding new loading stages
   #endregion
   
   public static ConsoleForm? ConsoleForm = null;
   public static Search? SearchForm = null;
   public static MapWindow MapWindow = null!;

   // SETTINGS
   public static readonly Settings Settings = new();
   public static ProvinceEditingStatus ProvinceEditingStatus = ProvinceEditingStatus.PreviewUntilSelection;

   // SELECTION
   public static Selection Selection = null!;

   // Date of history
   public static DateTime Date
   { 
      get
      {
         return MapWindow.DateControl.Date;
      }
   }
   // Logs
   public static readonly string DownloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
   public static readonly Log LoadingLog = new(Path.Combine(DownloadsFolder), "Loading"); //TODO: make this a setting and not hardcoded
   public static readonly Log ErrorLog = new(Path.Combine(DownloadsFolder), "Error"); //TODO: make this a setting and not hardcoded

   // Contains the current state of the application
   public static State State = State.Loading;
   public static EditingStatus EditingStatus = EditingStatus.Idle;
   public static MapModeRendering MapModeRendering { get; set; } = MapModeRendering.Live;
   public static bool AllowEditing => EditingStatus == EditingStatus.Idle;
   public static StripesDirection StripesDirection { get; set; } = StripesDirection.DiagonalLbRt;

   // History Manager
   public static readonly HistoryManager HistoryManager = new(new CInitial());

   // Contains the current map mode
   public static MapModeManager MapModeManager = null!;

   // Color Provider
   public static readonly ColorProviderRgb ColorProvider = new();

   // Contains the map image bounds and path
   public static int MapWidth;
   public static int MapHeight;
   public static string MapPath = null!;

   // ToolTip
   public static string ToolTipText = $"$MAPMODE_SPECIFIC$\n------------------\nId:   $id$\nName: $name$\nOwner: $owner$ ($owner%L$)\nArea: $area$ ($area%L$)";

   // Contains the border pixels of the provinces
   public static Point[] BorderPixels = [];
   public static Point[] Pixels = [];

   // Maps the name of TradeGoods to the TradeGood object
   public static readonly Dictionary<string, TradeGood> TradeGoods = [];
   public static readonly Dictionary<string, TradeNode> TradeNodes = [];

   // Culture Groups and Cultures
   public static Dictionary<string, CultureGroup> CultureGroups = [];
   public static Dictionary<string, Culture> Cultures = [];

   // Contains the provinces and options to access them
   public static Dictionary<int, Province> Provinces = [];
   public static Dictionary<Color, int> ColorToProvId = [];
   public static Dictionary<int, int[]> AdjacentProvinces = [];
   public static HashSet<int> Capitals = [];

   // TechnologyGroups
   public static readonly HashSet<string> TechnologyGroups = [];

   // Religion Stuff
   public static List<ReligiousGroup> ReligionGroups = [];
   public static readonly Dictionary<string, Religion> Religions = [];

   // Country Groups
   private static Dictionary<Tag, Country> _countries = [];
   public static Dictionary<Tag, Country> Countries
   {
      get => _countries;
      set
      {
         _countries = value;
         GlobalEventHandlers.RaiseCountryListChanged();
      } 
   }

   // Province Groups
   public static HashSet<int> LandProvinces = null!;
   public static HashSet<int> SeaProvinces = null!;
   public static HashSet<int> LakeProvinces = null!;
   public static HashSet<int> CoastalProvinces = null!;
   public static int[] NonLandProvinceIds = null!;
   public static int[] LandProvinceIds = null!;
   public static Dictionary<string, ColonialRegion> ColonialRegions = [];

   // Modifiers
   public static Dictionary<string, EventModifier> Modifiers = [];
   public static Dictionary<string, Modifier> ProvinceTriggeredModifiers = [];
   public static Dictionary<string, Modifier> TriggeredModifiers = [];

   // Trade
   public static Dictionary<string, TradeCompanyInvestment> TradeCompanyInvestments = [];
   public static Dictionary<string, TradeCompany> TradeCompanies = [];

   public static readonly Dictionary<string, int[]> ProvinceGroups = []; // TODO: read in
   // In Game Groups
   public static Dictionary<string, Area> Areas = [];
   public static Dictionary<string, Region> Regions { get; set; } = [];

   private static Dictionary<string, SuperRegion> _superRegions = [];
   public static Dictionary<string, SuperRegion> SuperRegions => _superRegions;
   public static void AddSuperRegion(SuperRegion sr)
   {
      if (_superRegions.TryAdd(sr.Name, sr)) 
         GlobalEventHandlers.RaiseSuperRegionListChanged(sr.Name, true);
   }
   public static void RemoveSuperRegion(string name)
   {
      if (_superRegions.Remove(name))
         GlobalEventHandlers.RaiseSuperRegionListChanged(name, false);
   }

   public static Dictionary<string, Continent> Continents { get; set; } = [];

   // Localisation
   public static Dictionary<string, string> Localisation { get; set; } = [];
   public static Dictionary<string, string> LocalisationCollisions { get; set; } = [];
   
   public static HashSet<string> ScriptedEffectNames {get; set; } = [];
   public static List<Building> Buildings { get; set; }= [];
   public static HashSet<string> BuildingKeys { get; set; }= [];

   // Is used in parsing province files as some keys differ from standard effect keys
   public static readonly HashSet<string> UniqueAttributeKeys = [
      "add_claim", "add_core", "add_local_autonomy", "add_nationalism", "base_manpower", "base_production", "base_tax", "capital", "center_of_trade", "controller", "culture", "discovered_by", "extra_cost", "hre", "city", "native_ferocity", "native_hostileness", "native_size", "owner", "religion", "seat_in_parliament", "trade_goods", "tribal_owner", "unrest", "shipyard", "revolt_risk", "is_city", "reformation_center", "citysize"
   ];

   public static List<string> ToolTippableAttributes = [ 
      "base_manpower", "base_tax", "base_production",  "total_development", "area", "continent", "claims", "cores", "controller", "owner", "tribal_owner", "center_of_trade", "extra_cost", "native_ferocity", "native_hostileness", "native_size", "revolt_risk", ">local_autonomy", "nationalism", "discovered_by", "capital", "culture", "religion", "has_fort_15th", "is_hre", "is_city", "is_seat_in_parliament", "trade_good", "history", "multiline_attributes", "id", "name", "has_revolt", "is_occupied"
   ];

   public static List<string> SelectionModifiers = [
      "Deselection"
   ];

   public static readonly HashSet<string> CountryAttributes = 
   [
      "cannot_form_from_collapse_nation", "right_to_bear_arms", "all_your_core_are_belong_to_us", "random_nation_extra_size", 
   ];

   private static Language _language = Language.english;
}

// TODO LIST Until Alpha 1.0
// - [x] Add a way to change the language of the application
// - [ ] Rework the Rendering of the map to be more efficient and using GDIP32
// - [ ] Add zooming to the map 0.5x - 16.0x
// - [ ] Add a way to change the map mode via customizable Hotkeys
// - [ ] Fix the Province Collection Editing gui so that all types work the same and edge cases are handled
// - [ ] Fix Lasso Selection Preview sometimes being incorrect
// - [ ] Fix Magic wand tool
// - [ ] Add a modifier creation and selection menu to apply to different scopes
// - [ ] Add saving for all Province Collections
// - [ ] Add descriptions on how to customize tooltips, map modes, and other things
// - [ ] Add basic country editing
// - [ ] Fix province center calculation
// - [ ] Find last concurrency bugs
// - [ ] Pre load and layout the Province Collection Editing GUI to prevent LagSpike on first opening said tab
// - [ ] MelonCoaster easter Egg in Loading Screen
// - [ ] Add a help page that leads to the official Discord
// - [ ] Improve selection modifiers and its GUI by adding more and only listing in context valid options
// - [ ] Straits editing and creation
// - [ ] Redo on create area does not work 
// - [ ] Redo on DeleteRegion still shows the region_name afterwards
// - [ ] Region creation is completely broken


// TODO LIST Until Alpha 1.1
// - [ ] Add a way to create custom map modes
// - [ ] Add a tradegoods creation and editing menu
