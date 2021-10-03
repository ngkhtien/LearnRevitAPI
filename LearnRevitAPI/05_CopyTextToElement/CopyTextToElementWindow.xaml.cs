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

namespace LearnRevitAPI._05_CopyTextToElement
{
   /// <summary>
   /// Interaction logic for CopyTextToElementWindow.xaml
   /// </summary>
   public partial class CopyTextToElementWindow
   {
      private CopyTextToElementViewModel _viewModel;
      private TransactionGroup transG;
      public CopyTextToElementWindow(CopyTextToElementViewModel viewModel)
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
         DialogResult = false;

         if (transG.HasStarted())
         {
            transG.RollBack();
            MessageBox.Show("Progress is Cancel!", "Stop Progress", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Stop);
         }
      }

      private void btnOK_Click(object sender, RoutedEventArgs e)
      {
         // Get elements to run
         IList<Element> allWalls = new FilteredElementCollector(_viewModel.Doc)
                                 .OfCategory(BuiltInCategory.OST_Walls)
                                 .WhereElementIsNotElementType()
                                 .ToList();

         // Setup progress bar
         ProgressBar.Maximum = allWalls.Count();

         List<ElementId> elementSuccess = new List<ElementId>();
         double valuePercent = 0;

         transG.Start("Run Process");

         foreach (Element wall in allWalls)
         {
            if (transG.HasStarted())
            {
               valuePercent += 1;
               _viewModel.Percent = valuePercent / ProgressBar.Maximum * 100;

               // Change number when run program
               ProgressBar.Dispatcher.Invoke(() => ProgressBar.Value = valuePercent, DispatcherPriority.Background);

               IList<TextNote> textNote = wall.GetTextNoteIntersectWithElement(_viewModel.Doc, _viewModel.Tolerance);

               if (textNote.Count == 0)
               {
                  continue;
               }

               using (Transaction trans = new Transaction(_viewModel.Doc))
               {
                  trans.Start("Copy text to Wall's Parameter");

                  // Delete warning
                  DeleteWarningSuper warningSuper = new DeleteWarningSuper();
                  FailureHandlingOptions failOpt = trans.GetFailureHandlingOptions();
                  failOpt.SetFailuresPreprocessor(warningSuper);
                  trans.SetFailureHandlingOptions(failOpt);

                  Parameter parameter = wall.LookupParameter(_viewModel.SelectedParameter);

                  if (parameter != null)
                  {
                     parameter.SetValue(textNote[0].Text);
                     elementSuccess.Add(wall.Id);
                  }

                  trans.Commit();
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

            MessageBox.Show(string.Concat("Success: ", elementSuccess.Count, " elements!"),
                   "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _viewModel.UiDoc.Selection.SetElementIds(elementSuccess);
         }
      }
   }
}
