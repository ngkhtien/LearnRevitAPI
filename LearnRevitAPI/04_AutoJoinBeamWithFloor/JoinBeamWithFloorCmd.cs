#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._04_AutoJoinBeamWithFloor
{
   [Transaction(TransactionMode.Manual)]
   public class JoinBeamWithFloorCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         using (TransactionGroup transGr = new TransactionGroup(doc))
         {
            transGr.Start("Auto Join Beam With Floor");

            JoinBeamWithFloorViewModel viewModel = new JoinBeamWithFloorViewModel(uidoc);
            JoinBeamWithFloorWindow window = new JoinBeamWithFloorWindow(viewModel);

            bool? showDialog = window.ShowDialog();

            if (showDialog == null || showDialog == false)
            {
               transGr.RollBack();
               return Result.Cancelled;
            }

            transGr.Assimilate();
            return Result.Succeeded;
         }
      }
   }
}
