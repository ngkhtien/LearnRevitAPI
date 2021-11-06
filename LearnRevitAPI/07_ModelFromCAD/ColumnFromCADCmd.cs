#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._07_ModelFromCAD
{
   [Transaction(TransactionMode.Manual)]
   class ColumnFromCADCmd: IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document Doc = uidoc.Document;

         // code

         using (TransactionGroup transGroup = new TransactionGroup(Doc))
         {
            transGroup.Start("Column From AutoCAD");

            ColumnFromCADViewModel viewModel = new ColumnFromCADViewModel(uidoc);

            ColumnFromCADWindow window = new ColumnFromCADWindow(viewModel);

            bool? showDialog = window.ShowDialog();

            if (showDialog == null || showDialog == false)
            {
               transGroup.RollBack();
               return Result.Cancelled;
            }

            transGroup.Assimilate();
            return Result.Succeeded;
         }
      }
   }
}
