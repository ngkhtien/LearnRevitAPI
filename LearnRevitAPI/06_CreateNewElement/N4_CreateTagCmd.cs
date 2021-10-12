#region Namespaces

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
   public class CreateTagCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         // Get tag symbol
         Element tagSymbol = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .Cast<FamilySymbol>()
            .Where(sym => sym.Category.Name.Equals("Wall Tags"))
            .FirstOrDefault();

         Reference r = uidoc.Selection.PickObject(ObjectType.Element, "Pick Wall");

         XYZ pt = uidoc.Selection.PickPoint("Pick Point To Tag");

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Wall Tag by Revit API");

            IndependentTag wallTag = IndependentTag.Create(doc, tagSymbol.Id, doc.ActiveView.Id, r, 
                                                           true, TagOrientation.Horizontal, pt);

            wallTag.TagHeadPosition = pt;

            // Set Leader Elbow
            BoundingBoxXYZ boxXyz = wallTag.get_BoundingBox(doc.ActiveView);
            double boxLength = boxXyz.Max.X - boxXyz.Min.X;

            XYZ elbow = wallTag.TagHeadPosition.Subtract(XYZ.BasisX.Multiply(boxLength / 2 +
                                                                             UnitUtils.Convert(700,
                                                                                DisplayUnitType.DUT_MILLIMETERS,
                                                                                DisplayUnitType.DUT_DECIMAL_FEET)));

            wallTag.LeaderElbow = elbow;

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
