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
         CreateRibbonPanel(a);

         return Result.Succeeded;
      }

      public Result OnShutdown(UIControlledApplication a)
      {
         return Result.Succeeded;
      }

      private void CreateRibbonPanel(UIControlledApplication a)
      {
         OutputConstraint constraint = new OutputConstraint();
         RibbonUtils ribbonUtils = new RibbonUtils(a.ControlledApplication);

         // Tạo Ribbon tab
         string ribbonName = "MEOS Learn RAPI";
         a.CreateRibbonTab(ribbonName);

         // Tạo Ribbon Panel 1
         string panelName = "Revit API Training 1";
         RibbonPanel panel1 = a.CreateRibbonPanel(ribbonName, panelName);
         // Tạo Ribbon Panel 2
         panelName = "Revit API Training 2";
         RibbonPanel panel2 = a.CreateRibbonPanel(ribbonName, panelName);

         string dllFile = "LearnRevitAPI.dll";

         //#region Create Push Button 

         PushButtonData pushButtonData
             = ribbonUtils.CreatePushButtonData("HelloWorldCmd",
                 "Hello\nWorld",
                 dllFile,
                 "LearnRevitAPI._01_HelloWorld.HelloWorldCmd",
                 "icons8-vietnam-32.png",
                 "This is tooltip",
                 constraint.HelperPath,
                 "This is long description",
                 "Lesson01_100px.png",
                 "https://youtu.be/wXsFlYz3uqA");

         panel1.AddItem(pushButtonData);


         //// Lesson04_SelectionFilteringCmd button
         //pushButtonData = ribbonUtils.CreatePushButtonData(
         //    "Lesson04_SelectionFilteringCmd",
         //    "Chọn\nĐối tượng",
         //    "AlphaBIM_RevitAPI_Training.dll",
         //    "AlphaBIM.Lesson04_SelectionFilteringCmd",
         //    "DungFiltersChonDoiTuong_32px.png",
         //    "This is tooltip",
         //    constraint.HelperPath,
         //    "This is long description",
         //    "DungFiltersChonDoiTuong_100px.png",
         //    "https://youtu.be/O8FKjaD5V80");
         //panel1.AddItem(pushButtonData);

         //#endregion Create Push Button
      }
   }
}
