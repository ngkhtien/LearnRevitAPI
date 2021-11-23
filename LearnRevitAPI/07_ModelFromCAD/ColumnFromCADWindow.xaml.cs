using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;
using System.Windows.Threading;
using LearnRevitAPI.Lib;
using System.Windows.Forms;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using Autodesk.Revit.DB.Structure;

namespace LearnRevitAPI._07_ModelFromCAD
{
   /// <summary>
   /// Interaction logic for ColumnFromCADWindow.xaml
   /// </summary>
   public partial class ColumnFromCADWindow
   {

      private ColumnFromCADViewModel _viewModel;
      private TransactionGroup transG;
      public ColumnFromCADWindow(ColumnFromCADViewModel viewModel)
      {
         InitializeComponent();

         _viewModel = viewModel;
         DataContext = viewModel;

         transG = new TransactionGroup(_viewModel.Doc);
      }

      #region Copy Title bar

      private void OpenWebSite(object sender, RoutedEventArgs e)
      {
         try
         {
            Process.Start("https://sites.google.com/view/revitmeos/home");
         }
         catch (Exception)
         {
         }
      }

      private void CustomDevelopment(object sender, RoutedEventArgs e)
      {
         try
         {
            Process.Start("https://sites.google.com/view/revitmeos/home");
         }
         catch (Exception)
         {
         }
      }

      private void Feedback(object sender, RoutedEventArgs e)
      {
         try
         {
            Process.Start("mailto:khtien0107@gmail.com");
         }
         catch (Exception)
         {
         }
      }

      #endregion Copy Title bar

      private void btnCancel_Click(object sender, RoutedEventArgs e)
      {
         if (transG.HasStarted())
         {
            DialogResult = false;
            transG.RollBack();
            System.Windows.MessageBox.Show("Progress is Cancel!", "Stop Progress",
                MessageBoxButton.OK, MessageBoxImage.Stop);
         }
      }

      private void btnOK_Click(object sender, RoutedEventArgs e)
      {
         // Get all planar face and data from planar face
         List<PlanarFace> hatchToCreateColumn = CadUtils.GetHatchHaveName(_viewModel.SelectedCadLink, _viewModel.SelectedLayer);

         List<ColumnData> allColumnDatas = new List<ColumnData>();

         foreach (PlanarFace hatch in hatchToCreateColumn)
         {
            ColumnData columnData = new ColumnData(hatch);
            allColumnDatas.Add(columnData);
         }

         ProgressBar.Maximum = allColumnDatas.Count;
         transG.Start("Run Process");

         List<ElementId> newColumns = new List<ElementId>();
         double value = 0;

         foreach (ColumnData columnData in allColumnDatas)
         {
            if (transG.HasStarted())
            {
               #region Setup for ProgressBar 

               value = value + 1;

               try
               {
                  Show();
               }
               catch (Exception)
               {
                  Close();
                  if (transG.HasStarted())
                  {
                     transG.RollBack();
                  }

                  System.Windows.MessageBox.Show("Progress is Cancel!", "Stop Progress",
                      MessageBoxButton.OK, MessageBoxImage.Stop);
                  break;
               }

               _viewModel.Percent = value / ProgressBar.Maximum * 100;

               ProgressBar.Dispatcher?.Invoke(() => ProgressBar.Value = value,
                   DispatcherPriority.Background);

               #endregion

               FamilySymbol familySymbol
                        = FamilySymbolUtils.GetFamilySymbolColumn(_viewModel.SelectedColumnFamily,
                            columnData.CanhNgan,
                            columnData.CanhDai,
                            "b", "h");

               if (familySymbol == null)
               {
                  continue;
               }

               using (Transaction tran = new Transaction(_viewModel.Doc, "Create Column"))
               {
                  tran.Start();

                  DeleteWarningSuper warningSuper = new DeleteWarningSuper();
                  FailureHandlingOptions failOpt = tran.GetFailureHandlingOptions();
                  failOpt.SetFailuresPreprocessor(warningSuper);
                  tran.SetFailureHandlingOptions(failOpt);

                  FamilyInstance instance = _viewModel.Doc.Create
                      .NewFamilyInstance(columnData.TamCot,
                      familySymbol, _viewModel.BaseLevel, StructuralType.Column);

                  instance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM)
                      .Set(_viewModel.BaseLevel.Id);

                  instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM)
                      .Set(_viewModel.TopLevel.Id);
                  
                  instance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM)
                      .Set(UnitUtils.Convert(_viewModel.BaseOffset, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET));

                  instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM)
                      .Set(UnitUtils.Convert(_viewModel.TopOffset, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET));

                  Line axis = Line.CreateUnbound(columnData.TamCot, XYZ.BasisZ);

                  ElementTransformUtils.RotateElement(_viewModel.Doc,
                      instance.Id,
                      axis,
                      columnData.GocXoay);

                  newColumns.Add(instance.Id);
                  tran.Commit();
               }
            }

            else
            {
               break;
            }
         }

         if (transG.HasStarted())
         {
            transG.Commit();
            DialogResult = true;

            MessageBox.Show(string.Concat("Success: ", newColumns.Count, " elements!"),
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _viewModel.UiDoc.Selection.SetElementIds(newColumns);
         }
      }
   }
}
