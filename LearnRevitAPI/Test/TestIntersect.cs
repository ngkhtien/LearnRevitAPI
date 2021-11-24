#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using LearnRevitAPI.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;
using ParameterUtils = LearnRevitAPI.Lib.ParameterUtils;

#endregion Namespaces

namespace LearnRevitAPI.Test
{
   [Transaction(TransactionMode.Manual)]
   class CheckIntersect : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         IList<Element> allBeams = new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_StructuralFraming)
                        .WhereElementIsNotElementType()
                        .ToList();

         //IList<Element> allWindows = new FilteredElementCollector(doc)
         //      .OfCategory(BuiltInCategory.OST_Windows)
         //      .WhereElementIsNotElementType()
         //      .ToList();


         foreach (Element beam in allBeams)
         {
            BoundingBoxXYZ beamBox = beam.get_BoundingBox(doc.ActiveView);
            Outline outline = new Outline(beamBox.Min, beamBox.Max);

            BoundingBoxIntersectsFilter bbfilter = new BoundingBoxIntersectsFilter(outline);

            ICollection<ElementId> idsExclude = new List<ElementId>();
            idsExclude.Add(beam.Id);
            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id)
               .OfCategory(BuiltInCategory.OST_Windows);
            collector.Excluding(idsExclude).WherePasses(bbfilter);

            int nCount = 0;
            string report = string.Empty;
            foreach (Element e in collector)
            {
               string name = e.Name;

               report += "\nName = " + name
                 + " Element Id: " + e.Id.ToString();

               nCount++;
            }

            if (nCount > 0)
            {
               TaskDialog.Show("Bounding Box + View + Exclusion Filter", "Found " + nCount.ToString()
                + " elements whose bounding box intersects" + report);

               uidoc.Selection.SetElementIds(collector.Select(e => e.Id).ToList());
            }
         }


         

         //using (Transaction trans = new Transaction(doc))
         //{
         //   trans.Start("Create share parameter");

         //   trans.Commit();
         //}

         return Result.Succeeded;
      }
   }
}