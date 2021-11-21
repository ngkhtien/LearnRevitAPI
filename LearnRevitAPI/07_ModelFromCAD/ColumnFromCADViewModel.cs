#region Namespaces

using System;
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

namespace LearnRevitAPI._07_ModelFromCAD
{
   public class ColumnFromCADViewModel : ViewModelBase
   {
      #region Field: save data from Revit to class ViewModel
      public Document Doc;
      public UIDocument UiDoc;
      #endregion

      public ColumnFromCADViewModel(UIDocument uiDoc)
      {
         // Save date from Revit to 2 fields Doc, UiDoc
         UiDoc = uiDoc;
         Doc = UiDoc.Document;

         Initialize();
      }

      private void Initialize()
      {
         // Select CAD Link
         Reference r = UiDoc.Selection.PickObject(ObjectType.Element, new ImportInstanceSelectionFilter(),
            "SELECT CAD LINK");

         SelectedCadLink = (ImportInstance)Doc.GetElement(r);

         // Get all layers
         AllLayers = CadUtils.GetAllLayer(SelectedCadLink);
         if (AllLayers.Any())
         {
            SelectedLayer = AllLayers[0];
         }

         // Get all column family
         AllColumnFamilies = new FilteredElementCollector(Doc)
            .OfClass(typeof(Family))
            .Cast<Family>()
            .Where(f => f.FamilyCategory.Name.Equals("Structural Columns")
                   || f.FamilyCategory.Name.Equals("Columns"))
            .ToList();

         if (AllColumnFamilies.Any())
         {
            SelectedColumnFamily = AllColumnFamilies[0];
         }

         // Get all levels
         AllLevel = new FilteredElementCollector(Doc)
            .OfClass(typeof(Level))
            .Cast<Level>()
            .ToList();

         BaseLevel = AllLevel.First();
         TopLevel = AllLevel.Last();
      }

      #region public property

      internal ImportInstance SelectedCadLink;

      #region Binding properties

      public List<string> AllLayers { get; set; }
      public string SelectedLayer { get; set; }
      public List<Family> AllColumnFamilies { get; set; }
      public Family SelectedColumnFamily { get; set; }
      public List<Level> AllLevel { get; set; }
      public Level BaseLevel { get; set; }
      public Level TopLevel { get; set; }
      public double BaseOffset { get; set; }
      public double TopOffset { get; set; }

      #endregion Binding properties
      public double Percent
      {
         get => _percent;
         set
         {
            _percent = value;

            // Thông báo cho WPF là property "Percent" đã thay đổi giá trị,
            // WPF hãy thay đổi theo
            OnPropertyChanged();
         }
      }

      #endregion public property

      #region private variable

      private double _percent;

      #endregion private variable
   }
}
