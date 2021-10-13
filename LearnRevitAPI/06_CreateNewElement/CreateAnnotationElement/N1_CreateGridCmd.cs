#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._06_CreateNewElement
{
   [Transaction(TransactionMode.Manual)]
   public class CreateGridCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         ObjectSnapTypes snapType = ObjectSnapTypes.Endpoints;

         XYZ point1 = uidoc.Selection.PickPoint(snapType, "Pick begin point");
         XYZ point2 = uidoc.Selection.PickPoint("Pick end point");

         Line gridLine = Line.CreateBound(point1, point2);

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Grid from Revit API");

            Grid newGrid = Grid.Create(doc, gridLine);
            newGrid.Name = "MeoCon";

            PreviewControl PRControl = new PreviewControl(doc, doc.ActiveView.Id);

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
