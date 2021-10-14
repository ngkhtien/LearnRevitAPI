#region Namespaces

using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._06_CreateNewElement.CreateFraming
{
   [Transaction(TransactionMode.Manual)]
   class CreateFramingCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         // Pick point
         XYZ pt1 = uidoc.Selection.PickPoint("Pick Start Point");
         XYZ pt2 = uidoc.Selection.PickPoint("Pick End Point");

         Curve framingLocation = Line.CreateBound(pt1, pt2);

         FamilySymbol familySymbol = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .OfCategory(BuiltInCategory.OST_StructuralFraming)
            .Cast<FamilySymbol>()
            .FirstOrDefault();

         // Get current level
         Level level = doc.ActiveView.GenLevel;

         // Get level
         // Level level = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().FirstOrDefault(l=>l.Name.Equal("Level 2"));

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Framing by Revit API");

            FamilyInstance instance =
               doc.Create.NewFamilyInstance(framingLocation, familySymbol, level, StructuralType.Beam);

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
