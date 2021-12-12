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
         if (_viewModel != null)
         {
            foreach (ViewExtension v in _viewModel.AllViewsExtension)
            {
               v.IsSelected = true;
            }
         }
      }

      private void SelectNone_Checked(object sender, RoutedEventArgs e)
      {
         foreach (ViewExtension v in _viewModel.AllViewsExtension)
         {
            v.IsSelected = false;
         }
      }

      private void btnClose_Click(object sender, RoutedEventArgs e)
      {
         //DialogResult = false;
         Close();
      }

      private void DeleteView_Click(object sender, RoutedEventArgs e)
      {
         //DialogResult = true;
         _viewModel.DeleteView();
      }

      private void IsSelectedClick(object sender, RoutedEventArgs e)
      {
         List<ViewExtension> selectedViews = new List<ViewExtension>();

         IList listViews = DataGridPurgeView.SelectedItems;
         foreach (object v in listViews)
         {
            selectedViews.Add(v as ViewExtension);
         }

         ViewExtension currentItem = DataGridPurgeView.CurrentItem as ViewExtension;

         ViewExtension first = selectedViews.FirstOrDefault(v => v.Equals(currentItem));
         if (first != null)
         {
            bool selected = first.IsSelected;
            foreach (ViewExtension vs in selectedViews)
               vs.IsSelected = !selected;
         }
      }
   }
}
