#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using LearnRevitAPI._02_SelectionFilter;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._06_CreateNewElement
{
   [Transaction(TransactionMode.Manual)]
   class CreateDimensionBetween2LinesCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         // Create line and reference
         XYZ pt1 = uidoc.Selection.PickPoint(ObjectSnapTypes.Endpoints, "Pick First Point");
         XYZ pt2 = uidoc.Selection.PickPoint(ObjectSnapTypes.Endpoints, "Pick Second Point");
         Line line = Line.CreateBound(pt1, pt2);

         Reference r1 = uidoc.Selection.PickObject(ObjectType.Element, new LineSelectionFilter(), "Choose First Line");
         Reference r2 =
            uidoc.Selection.PickObject(ObjectType.Element, new LineSelectionFilter(), "Choose Second Line");

         ReferenceArray referenceArray = new ReferenceArray();
         referenceArray.Append(r1);
         referenceArray.Append(r2);

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create New Dimension Between 2 Lines by Revit API");

            Dimension dimension = doc.Create.NewDimension(doc.ActiveView, line, referenceArray);

            XYZ translation = new XYZ(0,
               - UnitUtils.Convert(700, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET), 0);

            ElementTransformUtils.MoveElement(doc, dimension.Id, translation);

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
