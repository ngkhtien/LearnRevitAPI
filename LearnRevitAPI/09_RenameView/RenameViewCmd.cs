#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._09_RenameView
{
   [Transaction(TransactionMode.Manual)]
   public class RenameViewCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document Doc = uidoc.Document;

         using (TransactionGroup transGroup = new TransactionGroup(Doc))
         {
            transGroup.Start("Rename Views");

            RenameViewModel viewModel = new RenameViewModel(uidoc);
            RenameViewWindow window = new RenameViewWindow(viewModel);

            if (window.ShowDialog() == false)
            {
               return Result.Cancelled;
            }

            transGroup.Assimilate();
            return Result.Succeeded;
         }

      }
   }
}
