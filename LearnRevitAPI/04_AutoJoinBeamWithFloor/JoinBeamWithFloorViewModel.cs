using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using LearnRevitAPI.Lib;
using LearnRevitAPI.WPF_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnRevitAPI._04_AutoJoinBeamWithFloor
{
   public class JoinBeamWithFloorViewModel : ViewModelBase
   {
      #region Field: save data from Revit to class ViewModel
      public Document Doc;
      public UIDocument UiDoc;
      private double _percent;
      #endregion
      public JoinBeamWithFloorViewModel(UIDocument uidoc)
      {
         // Save date from Revit to 2 fields Doc, UiDoc
         Doc = uidoc.Document;
         UiDoc = uidoc;

         AllCategory = new List<string>() { "BEAM", "FLOOR" };

         PriorityCategory = AllCategory[0];
         SecondCategory = AllCategory[1];
         IsCurrentView = true;
      }

      #region Properties
      public List<string> AllCategory { get; set; }
      public string PriorityCategory { get; set; }
      public string SecondCategory { get; set; }
      public bool IsEntireProject { get; set; }
      public bool IsCurrentSelection { get; set; }
      public bool IsCurrentView { get; set; }

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
