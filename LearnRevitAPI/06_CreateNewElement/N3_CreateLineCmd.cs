#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces
namespace LearnRevitAPI._06_CreateNewElement
{
   [Transaction(TransactionMode.Manual)]
   class CreateLineCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         XYZ first = new XYZ(-17.9123197911809, 9.16512817383808, 0);
         XYZ second = new XYZ(first.X + UnitUtils.Convert(1000, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET), first.Y, first.Z);

         Line line = Line.CreateBound(first, second);

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Line from Revit API");

            DetailCurve detailCurve = doc.Create.NewDetailCurve(doc.ActiveView, line);

            uidoc.Selection.SetElementIds(new List<ElementId>() { detailCurve.Id });

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
