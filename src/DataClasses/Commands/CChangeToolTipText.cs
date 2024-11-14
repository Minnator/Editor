﻿namespace Editor.DataClasses.Commands;

public class CChangeToolTipText : ICommand
{
   private readonly string _oldToolTip;
   private readonly string _newToolTip;

   public CChangeToolTipText(string oldToolTip, string newToolTip, bool executeOnInit = true)
   {
      _oldToolTip = oldToolTip;
      _newToolTip = newToolTip;

      if (executeOnInit)
         Execute();
   }

   public void Execute()
   {
      Globals.Settings.ToolTip.ToolTipText = _newToolTip;
   }

   public void Undo()
   {
      Globals.Settings.ToolTip.ToolTipText = _oldToolTip;
   }

   public void Redo()
   {
      Execute();
   }

   public string GetDescription()
   {
      return $"Modified Tooltip string by [{_oldToolTip.Length - _newToolTip.Length}] chars";
   }
}