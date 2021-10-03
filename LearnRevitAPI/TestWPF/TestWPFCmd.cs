#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI.TestWPF
{
   [Transaction(TransactionMode.Manual)]
   public class TestWPFCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document Doc = uidoc.Document;

         TestWPFWindow testWPFWindow = new TestWPFWindow();

         bool? showDialog = testWPFWindow.ShowDialog();


         return Result.Succeeded;
      }
   }
}
