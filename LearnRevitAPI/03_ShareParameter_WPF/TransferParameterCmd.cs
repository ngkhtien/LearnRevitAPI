#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces


namespace LearnRevitAPI._03_ShareParameter_WPF
{
   [Transaction(TransactionMode.Manual)]
   public class TransferParameterCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         using (TransactionGroup transGr = new TransactionGroup(doc))
         {
            transGr.Start("Transfer Parameter");

            TransferParamerterViewModel viewModel = new TransferParamerterViewModel(uidoc);
            ParameterWindow window = new ParameterWindow(viewModel);

            if (window.ShowDialog() == false)
            {
               return Result.Cancelled;
            }

            transGr.Assimilate();
         }

         return Result.Succeeded;
      }
   }
}
