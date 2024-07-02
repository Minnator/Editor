﻿using System.Diagnostics;
using System.Windows.Forms;

namespace Editor.Forms
{
   public partial class ToolTipCustomizer : Form
   {
      public ToolTipCustomizer()
      {
         InitGui();
      }

      private void InitGui()
      {
         InitializeComponent();
         InputTextBox.AutoCompleteCustomSource.AddRange([.. Globals.ToolTippableAttributes]);
         ToolTipPreview.Columns.Add(new ColumnHeader("Tooltip Row"));
         ToolTipPreview.View = View.List;
      }

      private void AddButton_Click(object sender, System.EventArgs e)
      {
         if (InputTextBox.Text.Length == 0 || !IsValidToolTipString(InputTextBox.Text))
            return;
         ToolTipPreview.Items.Add(InputTextBox.Text);
         InputTextBox.Text = string.Empty;
      }

      private static bool IsValidToolTipString(string text)
      {
         var numOfDollarSigns = 0;
         foreach (var c in text)
         {
            if (c == '$')
               numOfDollarSigns++;
         }
         return numOfDollarSigns % 2 == 0;
      }

      private void RemoveButton_Click(object sender, System.EventArgs e)
      {
         if (ToolTipPreview.SelectedIndices.Count < 1)
            return;

         ToolTipPreview.Items.RemoveAt(ToolTipPreview.SelectedIndices[0]);
      }

      private void MoveUpButton_Click(object sender, System.EventArgs e)
      {
         if (ToolTipPreview.SelectedIndices.Count < 1)
            return;

         var index = ToolTipPreview.SelectedIndices[0];
         if (index == 0)
            return;

         var item = ToolTipPreview.Items[index];
         ToolTipPreview.Items.RemoveAt(index);
         ToolTipPreview.Items.Insert(index - 1, item);
         ToolTipPreview.SelectedIndices.Clear();
         ToolTipPreview.SelectedIndices.Add(index - 1);
      }

      private void MoveDownButton_Click(object sender, System.EventArgs e)
      {
         if (ToolTipPreview.SelectedIndices.Count < 1)
            return;

         var index = ToolTipPreview.SelectedIndices[0];
         if (index == ToolTipPreview.Items.Count - 1)
            return;

         var item = ToolTipPreview.Items[index];
         ToolTipPreview.Items.RemoveAt(index);
         ToolTipPreview.Items.Insert(index + 1, item);
         ToolTipPreview.SelectedIndices.Clear();
         ToolTipPreview.SelectedIndices.Add(index + 1);
      }

      private void ConfirmButton_Click(object sender, System.EventArgs e)
      {
         var str = string.Empty;
         foreach (ListViewItem item in ToolTipPreview.Items) 
            str += item.Text + "\n";
         Globals.ToolTipText = str.Trim();

         Debug.WriteLine(ToolTipBuilder.BuildToolTip(str, 1));
      }

      private void CancelButton_Click(object sender, System.EventArgs e)
      {
         Dispose();
      }
   }
}
