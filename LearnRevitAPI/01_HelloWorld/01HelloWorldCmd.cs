#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._01_HelloWorld
{
   [Transaction(TransactionMode.Manual)]
   internal class HelloWorldCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         // Select pick element
         Reference r = uidoc.Selection.PickObject(ObjectType.Element);
         Element e = doc.GetElement(r);

         // Get length of elemnt
         Parameter p = e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);

         // Show by Add-in Manager task dialog
         //TaskDialog.Show("message" , "Length = " +
         //                 UnitUtils.Convert(p.AsDouble(), DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS) + " mm");

         // Use windows forms system "System.Windows.Forms"
         // Remember to add "using Application = Autodesk.Revit.ApplicationServices.Application"
         MessageBox.Show("Length" +
                          UnitUtils.Convert(p.AsDouble(), DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS) + " mm");

         return Result.Succeeded;
      }
   }
}