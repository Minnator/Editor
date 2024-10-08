﻿using System.IO;

namespace Editor.DataClasses;

public class Log(string path, string logName)
{
   private int _logCount;
   private int _totalTime;
   private readonly StreamWriter _logFile = new(Path.Combine(path, logName + ".txt"));

   public void Write(string message)
   {
      lock (this)
      {
         _logFile.WriteLine(message);
         _logFile.Flush();
      }
   }

   public void WriteTimeStamp(string operation, long milliseconds, char fillCharacter = '.')
   {
      lock (this)
      {
         _logFile.WriteLine($"{operation.PadRight(40, fillCharacter)} [{milliseconds.ToString().PadLeft(6, fillCharacter)}]ms");
         _logFile.Flush();
         _logCount++;
         _totalTime += (int)milliseconds;
      }
   }

   public void Close()
   {
      _logFile.WriteLine("---------------------------------------------------");
      WriteTimeStamp("Total time", _totalTime);
      WriteTimeStamp("Total operations", _logCount);
      _logFile.Flush();
      _logFile.Close();
   }
}
