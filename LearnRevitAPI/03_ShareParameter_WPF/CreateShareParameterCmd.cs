#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using LearnRevitAPI.Lib;
using System.Collections.Generic;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion Namespaces

namespace LearnRevitAPI._03_ShareParameter_WPF
{
   [Transaction(TransactionMode.Manual)]
   class CreateShareParameterCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIApplication uiapp = commandData.Application;
         UIDocument uidoc = uiapp.ActiveUIDocument;
         Application app = uiapp.Application;
         Document doc = uidoc.Document;

         string sharedParaPath = OfficeUtils.GetPathDialog("Select share parameter",
                                                            string.Concat("All files|*.*", 
                                                            "|Text files|*.txt", 
                                                            "|Excel spreadsheet files|*.xls, *.xlsx"));

         string shareParaGroup = "REVIT API";
         string shareParaName = "Test API";
         string shareParaDesc = "Desc test API";

         List<Category> listCategory = new List<Category>();
         listCategory.Add(Category.GetCategory(doc, BuiltInCategory.OST_Walls));
         listCategory.Add(Category.GetCategory(doc, BuiltInCategory.OST_Floors));

         using (Transaction trans = new Transaction(doc))
         {
            trans.Start("Create share parameter");

            ParameterUtils.CreateSharedParamater(doc, app, sharedParaPath, shareParaGroup,
                                                 shareParaName, ParameterType.Area, BuiltInParameterGroup.PG_TEXT,
                                                 shareParaDesc, listCategory, true, true, true);

            trans.Commit();
         }

         return Result.Succeeded;
      }
   }
}
