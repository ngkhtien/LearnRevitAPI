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
using View = Autodesk.Revit.DB.View;

#endregion Namespaces

namespace LearnRevitAPI._08_PurgeView_DirectShape
{
   public class PurgeViewModel : ViewModelBase
   {
      #region Field: save data from Revit to class ViewModel
      public Document Doc;
      public UIDocument UiDoc;
      #endregion

      #region Binding Properties
      public List<ViewExtension> AllViewsExtension { get; set; }
      public List<ViewExtension> SelectedViewsExtension { get; set; }
      #endregion

      public PurgeViewModel(UIDocument uidoc)
      {
         Doc = uidoc.Document;
         UiDoc = uidoc;

         // Get all views
         List<View> allViews = new FilteredElementCollector(Doc)
            .OfCategory(BuiltInCategory.OST_Views)
            .Cast<View>()
            .Where(view => !view.IsTemplate
                           && view.ViewType != ViewType.Schedule
                           && view.ViewType != ViewType.DrawingSheet
                           && view.ViewType != ViewType.Internal
                           && view.ViewType != ViewType.ProjectBrowser
                           && view.ViewType != ViewType.SystemBrowser)
            .Where(v => v.Id != Doc.ActiveView.Id)
            .ToList();

         AllViewsExtension = new List<ViewExtension>();

         foreach (View view in allViews)
         {
            ViewExtension viewExtension = new ViewExtension(view);

            AllViewsExtension.Add(viewExtension);
         }

         AllViewsExtension.Sort((v1, v2) => string.CompareOrdinal(v1.Name, v2.Name));
      }
   }
}
