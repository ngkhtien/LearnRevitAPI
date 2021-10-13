#region Namespaces

using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces


namespace LearnRevitAPI._06_CreateNewElement
{
   [Transaction(TransactionMode.Manual)]
   class CreateDimensionForGridCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         // Create reference array for dimensions
         List<Grid> grids = uidoc.Selection.GetElementIds()
            .Select(id => doc.GetElement(id))
            .Where(e => e is Grid)
            .Cast<Grid>()
            .ToList();

         ReferenceArray referenceArray = new ReferenceArray();

         foreach (Grid grid in grids)
         {
            referenceArray.Append(new Reference(grid));
         }

         // Create line for dimension
         XYZ minimumPoint1 = grids[0].GetExtents().MinimumPoint;
         XYZ minumumPoint2 = grids[1].GetExtents().MinimumPoint;
         Line line = Line.CreateBound(minimumPoint1, minumumPoint2);

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Dimensions Between Grids by Revit API");

            DimensionType dimensionType = new FilteredElementCollector(doc)
               .OfClass(typeof(DimensionType))
               .Cast<DimensionType>()
               .FirstOrDefault();

            // Create dimension 1
            Dimension dimension = doc.Create.NewDimension(doc.ActiveView, line, referenceArray, dimensionType);

            XYZ translation = new XYZ(0,
               UnitUtils.Convert(800, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET), 0);

            ElementTransformUtils.MoveElement(doc, dimension.Id, translation);

            // Create dimension 2
            referenceArray = new ReferenceArray();
            //referenceArray.Append(new Reference(grids[0])); // way 1
            //referenceArray.Append(new Reference(grids[grids.Count - 1]));

            Element e = doc.GetElement(dimension.References.get_Item(0));
            referenceArray.Append(new Reference(e)); // way 2
            e = doc.GetElement(dimension.References.get_Item(dimension.References.Size - 1));
            referenceArray.Append(new Reference(e));

            dimension = doc.Create.NewDimension(doc.ActiveView, line, referenceArray, dimensionType);

            translation = new XYZ(0,
               UnitUtils.Convert(100, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET), 0);

            ElementTransformUtils.MoveElement(doc, dimension.Id, translation);

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
