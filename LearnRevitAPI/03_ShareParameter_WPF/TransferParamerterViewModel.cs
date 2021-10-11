using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using LearnRevitAPI.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParameterUtils = LearnRevitAPI.Lib.ParameterUtils;

namespace LearnRevitAPI._03_ShareParameter_WPF
{
   public class TransferParamerterViewModel
   {
      #region Field: save data from Revit to class ViewModel
      public Document Doc;
      public UIDocument UiDoc;
      #endregion

      public TransferParamerterViewModel(UIDocument uidoc)
      {
         // Save date from Revit to 2 fields Doc, UiDoc
         UiDoc = uidoc;
         Doc = UiDoc.Document;

         // Initialize data for WPF
         IList<Element> allElements = new FilteredElementCollector(Doc)
                                          .WhereElementIsNotElementType()
                                          .Where(e => e.Category != null)
                                          .Where(e => e.CanHaveAnalyticalModel()) // Remove line,.. element
                                          .ToList();

         allElements = allElements.Distinct(new EqualityCompareCategory()).ToList();

         // Get all parameter of element exclude type parameter
         AllSourceParameter = new List<string>();
         AllTargetParameter = new List<string>();

         foreach (Element e in allElements)
         {
            AllSourceParameter.AddRange(ParameterUtils.GetAllParameters(e, false));
            AllTargetParameter.AddRange(ParameterUtils.GetAllParametersEditable(e));
         }

         AllSourceParameter = AllSourceParameter.Distinct().ToList();
         AllSourceParameter.Sort();

         AllTargetParameter = AllTargetParameter.Distinct().ToList();
         AllTargetParameter.Sort();

         // Add default value for dialog
         SelectedSourceParameter = AllSourceParameter[0];
         SelectedTargetParameter = AllTargetParameter[0];
         IsCurrentView = true;
      }

      #region Properties
      public List<string> AllSourceParameter { get; set; }
      public string SelectedSourceParameter { get; set; }
      public List<string> AllTargetParameter { get; set; }
      public string SelectedTargetParameter { get; set; }
      public bool IsEntireProject { get; set; }
      public bool IsCurrentSelection { get; set; }
      public bool IsCurrentView { get; set; }
      #endregion

      public void TransferParameter()
      {
         // Get list element
         List<Element> allElementsToRun = new List<Element>();

         if (IsEntireProject)
         {
            allElementsToRun = new FilteredElementCollector(Doc)
                               .WhereElementIsNotElementType()
                               .Where(e => e.Category != null)
                               .ToList();
         }
         else if (IsCurrentView)
         {
            allElementsToRun = new FilteredElementCollector(Doc, Doc.ActiveView.Id)
                               .WhereElementIsNotElementType()
                               .Where(e => e.Category != null)
                               .ToList();
         }
         else if (IsCurrentSelection)
         {
            allElementsToRun = new FilteredElementCollector(Doc, UiDoc.Selection.GetElementIds())
                               .WhereElementIsNotElementType()
                               .Where(e => e.Category != null)
                               .ToList();
         }

         // Transfer value
         foreach (Element el in allElementsToRun)
         {
            Parameter sourceParameter = el.LookupParameter(SelectedSourceParameter);

            if (sourceParameter == null)
            {
               continue;
            }

            string sourceValue = sourceParameter.GetValue(true);

            Parameter targetParameter = el.LookupParameter(SelectedTargetParameter);

            if (targetParameter == null)
            {
               continue;
            }

            using (Transaction trans = new Transaction(Doc))
            {
               trans.Start("Transfer Parameter Value");
               targetParameter.SetValue(sourceValue);
               trans.Commit();
            }
         }

      }
   }
}
