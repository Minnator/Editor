﻿using System.Drawing;

namespace Editor.DataClasses;

public class Province
{
   public int Id { get; set; }
   public int SelfPtr { get; set; }
   public Color Color { get; set; }
   public int BorderPtr { get; set; }
   public int BorderCnt { get; set; }
   public int PixelPtr { get; set; }
   public int PixelCnt { get; set; }
   public Rectangle Bounds { get; set; }
   public Point Center { get; set; }
}