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
         DialogResult = true;
         //Close();

         foreach (ViewExtension2 v in _viewModel.AllViewsToRename)
         {
            using (Transaction trans = new Transaction(_viewModel.Doc))
            {
               trans.Start("x");
               try
               {
                  v.View.get_Parameter(BuiltInParameter.VIEW_NAME)
                      .Set(v.NewName);
               }
               catch (Exception exception)
               {
               }

               trans.Commit();
            }
         }
      }

      private void btnCancel_Click(object sender, RoutedEventArgs e)
      {
         DialogResult = false;
         Close();
      }

      private void btnRun_Click(object sender, RoutedEventArgs e)
      {
         foreach (var v in _viewModel.AllViewsToRename)
         {
            // Add prefix, suffix
            try
            {
               v.NewName
                   = _viewModel.Prefix + v.Name + _viewModel.Suffix;
            }
            catch (Exception exception)
            {
            }

            // find & replace
            if (string.IsNullOrEmpty(_viewModel.FindValue) == false)
            {
               v.NewName
                   = v.Name.Replace(_viewModel.FindValue,
                       _viewModel.ReplaceValue); ;
            }
         }
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
