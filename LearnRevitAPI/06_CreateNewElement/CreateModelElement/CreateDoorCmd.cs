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

namespace LearnRevitAPI._06_CreateNewElement.CreateModelElement
{
   [Transaction(TransactionMode.Manual)]
   class CreateDoorCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         // Pick point
         XYZ doorLocation = uidoc.Selection.PickPoint("Pick Door Location");

         FamilySymbol familySymbol = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .OfCategory(BuiltInCategory.OST_Doors)
            .Cast<FamilySymbol>()
            .FirstOrDefault();

         // Get door host
         Reference r = uidoc.Selection.PickObject(ObjectType.Element, "Pick Door Host");
         Element doorHost = doc.GetElement(r);

         // Get current level
         Level level = doc.ActiveView.GenLevel;

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create Door by Revit API");

            if (familySymbol.IsActive == false)
            {
               familySymbol.Activate();
            }

            FamilyInstance doorCreate = doc.Create.NewFamilyInstance(doorLocation, familySymbol, 
                                                                     doorHost, level, 
                                                                     StructuralType.NonStructural);

            doorCreate.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Create Door by Revit API");

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
