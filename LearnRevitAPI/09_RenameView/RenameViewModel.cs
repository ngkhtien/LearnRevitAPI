#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using LearnRevitAPI.Lib;
using LearnRevitAPI.WPF_Lib;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;
using ParameterUtils = LearnRevitAPI.Lib.ParameterUtils;
using View = Autodesk.Revit.DB.View;

#endregion Namespaces

namespace LearnRevitAPI._09_RenameView
{
   public class RenameViewModel : ViewModelBase
   {
      #region Field: save data from Revit to class ViewModel
      public Document Doc;
      public UIDocument UiDoc;
      public List<View> SelectedViews = new List<View>();
      #endregion

      #region Binding Properties
      public bool IsEntireProject
      {
         get => _isEntireProject;
         set
         {
            _isEntireProject = value;
            UpdateAllViewsExtensions();
         }
      }
      public bool IsCurrentSelection
      {
         get => _isCurrentSelection;
         set
         {
            _isCurrentSelection = value;
            UpdateAllViewsExtensions();
         }
      }
      public string Prefix { get; set; }
      public string Suffix { get; set; }
      public string FindValue { get; set; }
      public string ReplaceValue { get; set; }

      public ObservableCollection<ViewExtension2> AllViewsExtension { get; set; }
          = new ObservableCollection<ViewExtension2>();
      public List<ViewExtension2> SelectedViewsExtension { get; set; }

      public ObservableCollection<ViewExtension2> AllViewsToRename { get; set; }
          = new ObservableCollection<ViewExtension2>();

      public ObservableCollection<ViewExtension2> SelectedViewsToRename { get; set; }
          = new ObservableCollection<ViewExtension2>();
      #endregion

      #region private variable

      private bool _isEntireProject;
      private bool _isCurrentSelection;

      #endregion private variable

      public RenameViewModel(UIDocument uidoc)
      {
         Doc = uidoc.Document;
         UiDoc = uidoc;
         Initialize();
      }

      private void Initialize()
      {
         if (UiDoc.Selection.GetElementIds().Count > 0)
         {
            SelectedViews = new FilteredElementCollector(Doc, UiDoc.Selection.GetElementIds())
               .OfClass(typeof(View))
               .Cast<View>()
               .ToList();
         }

         if (SelectedViews.Count == 0)
         {
            IsEntireProject = true;
         }
         else
         {
            IsCurrentSelection = true;
         }
      }
      private void UpdateAllViewsExtensions()
      {
         ViewExtension2 level1 = new ViewExtension2("Views (all)");

         List<ViewType> viewTypes = ViewUtils.GetAllViewsType(Doc, IsCurrentSelection, SelectedViews);

         foreach (ViewType viewType in viewTypes)
         {
            List<View> allViewOfViewType = ViewUtils.GetAllViewsWithViewType(Doc, viewType,
               IsCurrentSelection, SelectedViews);

            if (allViewOfViewType.Count() == 0)
            {
               continue;
            }

            ViewExtension2 level2 = new ViewExtension2(viewType);

            level1.ViewItems.Add(level2);

            foreach (View view in allViewOfViewType)
            {
               ViewExtension2 level3 = new ViewExtension2(view);
               level2.ViewItems.Add(level3);
            }
         }

         AllViewsExtension = new ObservableCollection<ViewExtension2>();
         AllViewsExtension.Add(level1);

         OnPropertyChanged("AllViewsExtension");
      }

      public void AddViews()
      {
         foreach (ViewExtension2 view in AllViewsExtension)
         {
            ViewExtension2 level1 = view;
            List<ViewExtension2> level2 = level1.ViewItems.ToList();

            foreach (ViewExtension2 lv2 in level2)
            {
               List<ViewExtension2> views = new List<ViewExtension2>(lv2.ViewItems.ToList());

               foreach (ViewExtension2 v in views)
               {
                  if (v.IsSelected)
                  {
                     AllViewsToRename.Add(v);
                     lv2.ViewItems.Remove(v);
                  }
               }

               if (lv2.ViewItems.Any() == false)
               {
                  level1.ViewItems.Remove(lv2);
               }
            }
         }
      }

      public void RemoveViews()
      {
         ViewExtension2 level1 = AllViewsExtension[0];
         List<ViewExtension2> level2List = level1.ViewItems.ToList();

         // Nếu remove trực tiếp property SelectedViewsToRename thì sẽ báo lỗi
         List<ViewExtension2> selectedViewsToRename
            = new List<ViewExtension2>(SelectedViewsToRename);

         foreach (ViewExtension2 v in selectedViewsToRename)
         {
            AllViewsToRename.Remove(v);

            bool isHaveLevel2 = false;
            foreach (ViewExtension2 lv2 in level2List)
            {
               if (lv2.Name.Equals(v.ViewType.ToString()))
               {
                  lv2.ViewItems.Add(v);
                  isHaveLevel2 = true;
                  break;
               }
            }

            if (!isHaveLevel2)
            {
               ViewExtension2 lv2
                   = new ViewExtension2(v.ViewType);
               level1.ViewItems.Add(lv2);
               level2List = level1.ViewItems.ToList();
               lv2.ViewItems.Add(v);
            }
         }
      }

   }
}
