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
         throw new NotImplementedException();
      }

      private void btnOK_Click(object sender, RoutedEventArgs e)
      {
         throw new NotImplementedException();
      }
   }
}
