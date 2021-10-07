#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._06_CreateNewElement
{
   [Transaction(TransactionMode.Manual)]
   public class CreateLevelCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Level from Revit API");

            double levelElevation = UnitUtils.Convert(5000, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET);
            Level newLevel = Level.Create(doc, levelElevation);
            newLevel.Name = "MeoCon";

            ViewFamilyType viewFamilyType = new FilteredElementCollector(doc)
                                    .OfClass(typeof(ViewFamilyType))
                                    .Cast<ViewFamilyType>()
                                    .FirstOrDefault(q => q.ViewFamily == ViewFamily.FloorPlan);

            ViewPlan.Create(doc, viewFamilyType.Id, newLevel.Id);

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
