﻿using System.Drawing.Imaging;
using Editor.Controls;
using Editor.DataClasses.GameDataClasses;
using Editor.Helper;

namespace Editor.Forms
{
   public partial class SelectionDrawerForm : Form
   {
      private SelectionExportSettings ExportSettings { get; set; } = new();
      private ZoomControl ZoomControl { get; set; } = new(new(Globals.MapWidth, Globals.MapHeight, PixelFormat.Format32bppArgb));

      public SelectionDrawerForm()
      {
         InitializeComponent();

         MainLayoutPanel.Controls.Add(ZoomControl, 1, 0);
         ZoomControl.FocusOn(new Rectangle(0, 0, Globals.MapWidth, Globals.MapHeight));

         ExportSettingsPropertyGrid.PropertyValueChanged += ExportSettingsPropertyChanged;
         ExportSettingsPropertyGrid.SelectedObject = ExportSettings;

         Globals.MapModeManager.MapModeChanged += (s, e) => RenderImage();
         Selection.OnProvinceGroupDeselected += (s, e) =>
         {
            RenderImage();
         };
         Selection.OnProvinceGroupSelected += (s, e) => RenderImage();
      }

      private void ExportSettingsPropertyChanged(object? s, PropertyValueChangedEventArgs e)
      {
         ExportSettings = (SelectionExportSettings)ExportSettingsPropertyGrid.SelectedObject;
         RenderImage();
      }

      private void SelectFolderButton(object sender, EventArgs e)
      {
         IO.OpenFolderDialog(Globals.ModPath, "select a folder where to save the image", out var path);
         PathTextBox.Text = path;
      }

      private void RenderImage()
      {
         MapDrawing.Clear(ZoomControl, Color.DimGray);
         switch (ExportSettings.PrimaryProvinceDrawing)
         {
            case PrimaryProvinceDrawing.None:
               break;
            case PrimaryProvinceDrawing.Selection:
               MapDrawing.DrawOnMap(Selection.GetSelectedProvinces, Globals.MapModeManager.GetMapModeColor, ZoomControl, PixelsOrBorders.Both);
               break;
            case PrimaryProvinceDrawing.Land:
               MapDrawing.DrawOnMap(Globals.LandProvinces, Globals.MapModeManager.GetMapModeColor, ZoomControl, PixelsOrBorders.Both);
               break;
            case PrimaryProvinceDrawing.All:
               MapDrawing.DrawOnMap(Globals.Provinces, Globals.MapModeManager.GetMapModeColor, ZoomControl, PixelsOrBorders.Both);
               break;
         }

         DrawSecondary(ExportSettings.SecondaryProvinceDrawing);

         DrawSecondary(ExportSettings.TertiaryProvinceDrawing);

         switch (ExportSettings.BorderDrawing)
         {
            case BorderDrawing.None:
               break;
            case BorderDrawing.Selection:
               MapDrawing.DrawOnMap(Selection.GetSelectedProvinces, Color.Black.ToArgb(), ZoomControl, PixelsOrBorders.Borders);
               break;
            case BorderDrawing.All:
               MapDrawing.DrawAllBorders(Color.Black.ToArgb(), ZoomControl);
               break;
         }

         if (Selection.Count == 0)
            return;

         ZoomControl.FocusOn(Geometry.GetBounds(Selection.GetSelectedProvinces));
         ZoomControl.Invalidate();
      }

      private void DrawSecondary(SecondaryProvinceDrawing secondary)
      {
         switch (secondary)
         {
            case SecondaryProvinceDrawing.None:
               break;
            case SecondaryProvinceDrawing.NeighboringProvinces:
               var neighboringProvinces = Geometry.GetAllNeighboringProvinces(Selection.GetSelectedProvinces);
               MapDrawing.DrawOnMap(neighboringProvinces, Globals.MapModeManager.GetMapModeColor, ZoomControl, PixelsOrBorders.Both);
               break;
            case SecondaryProvinceDrawing.NeighboringCountries:
               var neighboringCountries = Geometry.GetAllNeighboringCountries(Selection.GetSelectedProvinces);
               List<Province> allCountryProvinces = [];
               foreach (var country in neighboringCountries)
               {
                  var provinces = Globals.Countries[country].GetProvinces().ToList();
                  foreach (var province in provinces)
                     allCountryProvinces.Add(province);
               }
               MapDrawing.DrawOnMap(allCountryProvinces, Globals.MapModeManager.GetMapModeColor, ZoomControl, PixelsOrBorders.Both);
               break;
            case SecondaryProvinceDrawing.CoastalOutline:
            // TODO calculate coastlines
            case SecondaryProvinceDrawing.SeaProvinces:
               MapDrawing.DrawOnMap(Globals.SeaProvinces, Globals.MapModeManager.GetMapModeColor, ZoomControl, PixelsOrBorders.Both);
               break;
            case SecondaryProvinceDrawing.All:
               MapDrawing.DrawOnMap(Globals.Provinces, Globals.MapModeManager.GetMapModeColor, ZoomControl, PixelsOrBorders.Both);
               break;
         }
      }

      private void SaveButton_Click(object sender, EventArgs e)
      {
         using var bmp = ZoomControl.Map;
         switch (ExportSettings.ImageSize)
         {
            case ImageSize.Original:
               bmp.Save(Path.Combine(PathTextBox.Text, $"{Globals.MapModeManager.CurrentMapMode.GetMapModeName()}.png"), ImageFormat.Png);
               break;
            case ImageSize.Selection:
               var rect = Geometry.GetBounds(Selection.GetSelectedProvinces);
               var bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
               // copy the selected area to the new bitmap
               using (var g = Graphics.FromImage(bitmap))
               {
                  g.DrawImage(bmp, rect with { X = 0, Y = 0 }, rect, GraphicsUnit.Pixel);
               }
               bitmap.Save(Path.Combine(PathTextBox.Text, $"{Globals.MapModeManager.CurrentMapMode.GetMapModeName()}.png"), ImageFormat.Png);
               bitmap?.Dispose();
               break;
         }
      }

      private void SelectionDrawerForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         ZoomControl.Dispose();
      }
   }

   public enum ImageSize
   {
      Original,
      Selection,

   }

   public enum BorderDrawing
   {
      None,
      Selection,
      All
   }

   public enum PrimaryProvinceDrawing
   {
      None,
      Selection,
      Land,
      All
   }

   public enum SecondaryProvinceDrawing
   {
      None,
      NeighboringProvinces,
      NeighboringCountries,
      SeaProvinces,
      CoastalOutline,
      All
   }


   public class SelectionExportSettings
   {
      public BorderDrawing BorderDrawing { get; set; }
      public PrimaryProvinceDrawing PrimaryProvinceDrawing { get; set; }
      public SecondaryProvinceDrawing SecondaryProvinceDrawing { get; set; }
      public SecondaryProvinceDrawing TertiaryProvinceDrawing { get; set; }
      public ImageSize ImageSize { get; set; }
      public Color BackColor { get; set; } = Color.Black;
   }
}
