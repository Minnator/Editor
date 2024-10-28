﻿using Editor.DataClasses;
using Editor.DataClasses.GameDataClasses;
using Editor.Interfaces;

namespace Editor.Helper;

public static class Localisation
{
   private static readonly LocObject SearchLoc = new("", string.Empty, ObjEditingStatus.Immutable);
   public static string GetLoc (string key)
   {
      SearchLoc.Key = key;
      return GetLocObject(key, out var value) ? value.Value : key;
   }

   public static bool GetLocObject(string key, out LocObject? locObject)
   {
      SearchLoc.Key = key;
      return Globals.Localisation.TryGetValue(SearchLoc, out locObject);
   }

   /// <summary>
   /// If the value has not changed, it will not be added or updated
   /// If the value has changed, it will be updated
   /// If the key does not exist, it will be added
   /// </summary>
   /// <param name="key"></param>
   /// <param name="value"></param>
   public static void AddOrModifyLocObject(string key, string value)
   {
      if (!GetLocObject(key, out var locObject))
      {
         if (!string.IsNullOrEmpty(value))
            Globals.Localisation.Add(new(key, value));
      }
      else
      {
         if (locObject.Path.isModPath && string.IsNullOrEmpty(value))
         {
            // TODO delete locObject
         }
         locObject.Value = value;
      }
   }
}

/// <summary>
/// The value will never be updated if it would not changed
/// </summary>
public class LocObject : Saveable
{
   public LocObject(string key, string value, ObjEditingStatus status = ObjEditingStatus.Modified)
   {
      Key = key;
      _value = value;
      EditingStatus = status;
   }
   public string Key { get; set; }
   private string _value;

   public string Value
   {
      get => _value;
      set {
         if (value == _value)
            return;
         _value = value;
         EditingStatus = ObjEditingStatus.Modified;
      }
   }

   public sealed override ObjEditingStatus EditingStatus
   {
      get => _editingStatus;
      set
      {
         if (_editingStatus == ObjEditingStatus.Immutable)
            return;
         if (Equals(value, _editingStatus))
            return;
         if (Equals(value, ObjEditingStatus.Modified))
            FileManager.AddLocObject(this);
         _editingStatus = value;
      }
   }

   public override ModifiedData WhatAmI()
   {
      return ModifiedData.Localisation;
   }

   public override string SavingComment()
   {
      return string.Empty;
   }

   public override PathObj GetDefaultSavePath()
   {
      return new(["localisation"]);
   }

   public override string GetSaveString(int tabs)
   {
      return $"{Key}:0 \"{Value}\"";
   }

   public override string GetSavePromptString()
   {
      return $"localisation: \"{Key}\"";
   }

   public override int GetHashCode()
   {
      return Key.GetHashCode();
   }

   public override bool Equals(object? obj)
   {
      if (obj is not LocObject locObject) 
         return false;
      return Key.Equals(locObject.Key);
   }
}
