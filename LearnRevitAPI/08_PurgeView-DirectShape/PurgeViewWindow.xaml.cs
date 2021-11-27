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

namespace LearnRevitAPI._08_PurgeView_DirectShape
{
   /// <summary>
   /// Interaction logic for PurgeViewWindow.xaml
   /// </summary>
   public partial class PurgeViewWindow
   {
      private PurgeViewModel _viewModel;
      private TransactionGroup transG;

      public PurgeViewWindow(PurgeViewModel viewModel)
      {
         InitializeComponent();

         _viewModel = viewModel;
         DataContext = viewModel;

         //transG = new TransactionGroup(_viewModel.Doc);
      }

      private void SelectAll_Checked(object sender, RoutedEventArgs e)
      {
         
      }

      private void SelectNone_Checked(object sender, RoutedEventArgs e)
      {
         
      }

      private void btnClose_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void DeleteView_Click(object sender, RoutedEventArgs e)
      {
         
      }

      private void IsSelectedClick(object sender, RoutedEventArgs e)
      {
         
      }
   }
}
