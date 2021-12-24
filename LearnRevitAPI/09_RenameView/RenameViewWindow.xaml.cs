using System;
using System.Collections;
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

namespace LearnRevitAPI._09_RenameView
{
   /// <summary>
   /// Interaction logic for RenameViewWindow.xaml
   /// </summary>
   public partial class RenameViewWindow
   {
      private RenameViewModel _viewModel;

      public RenameViewWindow(RenameViewModel viewModel)
      {
         InitializeComponent();

         _viewModel = viewModel;
         DataContext = viewModel;
      }

      private void btnOk_Click(object sender, RoutedEventArgs e)
      {

      }

      private void btnCancel_Click(object sender, RoutedEventArgs e)
      {

      }

      private void btnRun_Click(object sender, RoutedEventArgs e)
      {

      }

      private void btnAddViews_Click(object sender, RoutedEventArgs e)
      {
         _viewModel.AddViews();
      }

      private void btnRemoveViews_Click(object sender, RoutedEventArgs e)
      {
         _viewModel.RemoveViews();
      }

      private void TreeView_OnChecked(object sender, RoutedEventArgs e)
      {
         // code
      }
   }
}
