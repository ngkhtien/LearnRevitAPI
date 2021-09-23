#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

#endregion

namespace LearnRevitAPI._02_SelectionFilter
{
   [Transaction(TransactionMode.Manual)]
   class SelectionFilterCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         #region 1: uidoc.Selection

         #region 1.1: Get selected elements

         //List<ElementId> elementIds = uidoc.Selection.GetElementIds().ToList();
         //string elementsName = string.Empty;

         //foreach (ElementId eId in elementIds)
         //{
         //   Element e = doc.GetElement(eId);
         //   elementsName = elementsName + e.Name + "\n";
         //}

         //MessageBox.Show(elementsName);

         #endregion

         #region 1.2: Pick Object

         #region 1.2.1: Pick 1 Object, show Name
         //Reference r = uidoc.Selection.PickObject(ObjectType.Element, new WallSelectionFilter(), "Pick Wall");
         //Element e = doc.GetElement(r);

         //MessageBox.Show(e.Name);
         #endregion

         #region 1.2.2: Pick Objects, show Volume
         //IList<Reference> pickObjects = uidoc.Selection.PickObjects(ObjectType.Element, new FloorSelectionFilter(), "Pick Floors");

         //foreach (Reference r in pickObjects)
         //{
         //   Element e = doc.GetElement(r);
         //   Parameter p = e.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
         //   double volumeMetre = UnitUtils.Convert(p.AsDouble(), DisplayUnitType.DUT_CUBIC_FEET, DisplayUnitType.DUT_CUBIC_METERS);

         //   MessageBox.Show(volumeMetre.ToString());
         //}

         #endregion

         #endregion

         #region 2: FilteredElementCollector

         #region 2.1: Use one filter
         //List<Floor> allFloors = new FilteredElementCollector(doc)
         //                            .OfClass(typeof(Floor))
         //                            .Cast<Floor>()
         //                            .ToList();

         //List<Floor> allFloors = new FilteredElementCollector(doc, uidoc.Selection.GetElementIds())
         //                            .OfClass(typeof(Floor))
         //                            .Cast<Floor>()
         //                            .ToList();

         //MessageBox.Show(allFloors.Count().ToString());

         //uidoc.Selection.SetElementIds(allFloors.Select(e => e.Id).ToList());

         //List<Element> allColums = new FilteredElementCollector(doc)
         //                          .WhereElementIsNotElementType()
         //                          .OfCategory(BuiltInCategory.OST_StructuralColumns)
         //                          .ToElements()
         //                          .ToList();

         //MessageBox.Show(allColums.Count().ToString());

         //uidoc.Selection.SetElementIds(allColums.Select(e => e.Id).ToList());
         #endregion

         #region 2.2: Use more than 1 filters (ElementLogicalFilter)
         //FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);

         #region 2.2.1: LogicalOrFilter
         //View3D view3d = new FilteredElementCollector(doc)
         //                .OfClass(typeof(View3D))
         //                .Cast<View3D>()
         //                .FirstOrDefault(v => v.Name.Equals("3D Test"));

         //if (view3d == null)
         //{
         //   MessageBox.Show("No view 3D Test");
         //   return Result.Cancelled;
         //}

         //FilteredElementCollector collector = new FilteredElementCollector(doc, view3d.Id);

         //ElementCategoryFilter wallFilter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
         //ElementClassFilter floorFilter = new ElementClassFilter(typeof(Floor));

         //IList<ElementFilter> filters = new List<ElementFilter>();
         //filters.Add(wallFilter);
         //filters.Add(floorFilter);

         //LogicalOrFilter orFilter = new LogicalOrFilter(filters);

         //IList<Element> listElement = collector.WherePasses(orFilter).WhereElementIsNotElementType().ToElements();

         //MessageBox.Show(listElement.Count().ToString());
         //uidoc.Selection.SetElementIds(listElement.Select(e => e.Id).ToList());
         #endregion

         #region 2.2.2: LogicalAndFilter
         //View3D view3d = new FilteredElementCollector(doc)
         //                .OfClass(typeof(View3D))
         //                .Cast<View3D>()
         //                .FirstOrDefault(v => v.Name.Equals("3D Test"));

         //if (view3d == null)
         //{
         //   MessageBox.Show("No view 3D Test");
         //   return Result.Cancelled;
         //}

         //FilteredElementCollector collector = new FilteredElementCollector(doc, view3d.Id);

         //Level lv = new FilteredElementCollector(doc)
         //           .OfClass(typeof(Level))
         //           .Cast<Level>()
         //           .FirstOrDefault(l => l.Name.Equals("Level 1"));

         //ElementClassFilter floorFilter = new ElementClassFilter(typeof(Floor));
         //ElementLevelFilter levelFilter = new ElementLevelFilter(lv.Id);

         //LogicalAndFilter andFilter = new LogicalAndFilter(floorFilter, levelFilter);

         //IList<Element> listElement = collector.WherePasses(andFilter).ToElements();

         //MessageBox.Show(listElement.Count().ToString());
         //uidoc.Selection.SetElementIds(listElement.Select(e => e.Id).ToList());
         #endregion

         #region 2.2.3: ElementMultiCategoryFilter, ElementMultiClassFilter

         #region ElementMultiCategoryFilter
         //FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);

         //ICollection<BuiltInCategory> categories = new List<BuiltInCategory>();
         //categories.Add(BuiltInCategory.OST_Floors);
         //categories.Add(BuiltInCategory.OST_Walls);

         //ElementMulticategoryFilter multicategoryFilter = new ElementMulticategoryFilter(categories);

         //IList<Element> listElement = collector.WherePasses(multicategoryFilter)
         //                                      .WhereElementIsNotElementType()
         //                                      .ToElements();

         //MessageBox.Show(listElement.Count().ToString());
         //uidoc.Selection.SetElementIds(listElement.Select(e => e.Id).ToList());
         #endregion

         #region ElementMultiClassFilter
         FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);

         IList<Type> typeList = new List<Type>();
         typeList.Add(typeof(Floor));
         typeList.Add(typeof(Wall));

         ElementMulticlassFilter multiclassFilter = new ElementMulticlassFilter(typeList);

         IList<Element> listElement = collector.WherePasses(multiclassFilter)
                                               .WhereElementIsNotElementType()
                                               .ToElements();

         MessageBox.Show(listElement.Count().ToString());
         uidoc.Selection.SetElementIds(listElement.Select(e => e.Id).ToList());
         #endregion

         #endregion
         #endregion
         #endregion
         #endregion

         return Result.Succeeded;
      }
   }
}
