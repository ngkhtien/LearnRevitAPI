#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

#endregion

namespace LearnRevitAPI
{
   class App : IExternalApplication
   {
      public Result OnStartup(UIControlledApplication a)
      {
         return Result.Succeeded;
      }

      public Result OnShutdown(UIControlledApplication a)
      {
         return Result.Succeeded;
      }
   }
}
