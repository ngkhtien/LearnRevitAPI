using System.Collections.ObjectModel;
using Autodesk.Revit.DB;
using LearnRevitAPI.WPF_Lib;

namespace LearnRevitAPI
{
   public class ViewExtension2 : ViewModelBase
   {
      public View View { get; set; }
      public ViewType ViewType { get; set; }

      public ObservableCollection<ViewExtension2> ViewItems { get; set; }

      public string Name
      {
         get => _name;
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }
      public string NewName
      {
         get => _newName;
         set
         {
            _newName = value;
            OnPropertyChanged();
         }
      }
      public bool IsSelected
      {
         get => _isSelected;

         set
         {
            _isSelected = value;
            OnPropertyChanged();
         }
      }

      public ViewExtension2(View view)
      {
         ViewItems = new ObservableCollection<ViewExtension2>();
         View = view;
         Name = view.get_Parameter(BuiltInParameter.VIEW_NAME).AsString();
         ViewType = view.ViewType;
         IsSelected = false;
      }
      public ViewExtension2(string name)
      {
         ViewItems = new ObservableCollection<ViewExtension2>();
         Name = name;
         IsSelected = false;
      }
      public ViewExtension2(ViewType viewType)
      {
         ViewItems = new ObservableCollection<ViewExtension2>();
         Name = viewType.ToString();
         ViewType = viewType;
         IsSelected = false;
      }

      private bool _isSelected;
      private string _name;
      private string _newName;
   }
}