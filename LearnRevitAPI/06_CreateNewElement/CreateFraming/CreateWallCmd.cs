#region Namespaces

using System.Collections;
using System.Collections.Generic;
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
   class CreateWallCmd : IExternalCommand
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

         IList<Curve> wallPlace = new List<Curve>();

         Curve wallLocation = Line.CreateBound(pt1, pt2);
         wallPlace.Add(wallLocation);

         WallType wallType = new FilteredElementCollector(doc)
            .OfClass(typeof(WallType))
            .OfCategory(BuiltInCategory.OST_Walls)
            .Cast<WallType>()
            .FirstOrDefault();

         // Get current level
         Level level = doc.ActiveView.GenLevel;

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Wall by Revit API");

            //FamilyInstance instance =
            //   doc.Create.NewFamilyInstance(framingLocation, familySymbol, level, StructuralType.NonStructural);

            Wall wall = Wall.Create(doc, wallLocation, wallType.Id, level.Id, 2000, 0, false, false);

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
