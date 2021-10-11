#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using LearnRevitAPI.Lib;
using LearnRevitAPI.WPF_Lib;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;
using ParameterUtils = LearnRevitAPI.Lib.ParameterUtils;

#endregion Namespaces

namespace LearnRevitAPI._05_CopyTextToElement
{
   public class CopyTextToElementViewModel : ViewModelBase
   {
      #region Field: save data from Revit to class ViewModel
      public Document Doc;
      public UIDocument UiDoc;
      private double _percent;
      #endregion
      public CopyTextToElementViewModel(UIDocument uiDoc)
      {
         // Save date from Revit to 2 fields Doc, UiDoc
         UiDoc = uiDoc;
         Doc = UiDoc.Document;

         // Initialize data for WPF
         IList<Element> allElements = new FilteredElementCollector(Doc)
                                          .OfCategory(BuiltInCategory.OST_Walls)
                                          .WhereElementIsNotElementType()
                                          .ToList();

         // Get all parameter of element exclude type parameter
         AllParameter = new List<string>();

         AllParameter.AddRange(ParameterUtils.GetAllStringParametersEditable(allElements[0]));
         AllParameter.Sort();

         // Add default value for dialog
         SelectedParameter = AllParameter[0];
         Tolerance = 50;
      }

      #region Properties
      public List<string> AllParameter { get; set; }
      public string SelectedParameter { get; set; }
      public double Tolerance { get; set; }
      public double Percent
      {
         get => _percent;

         set
         {
            _percent = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
