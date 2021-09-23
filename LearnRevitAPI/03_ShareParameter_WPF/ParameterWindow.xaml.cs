using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LearnRevitAPI._03_ShareParameter_WPF
{
   /// <summary>
   /// Interaction logic for ParameterWindow.xaml
   /// </summary>
   public partial class ParameterWindow
   {
      private TransferParamerterViewModel _viewModel;
      public ParameterWindow(TransferParamerterViewModel viewModel)
      {
         InitializeComponent();

         _viewModel = viewModel;
         DataContext = viewModel;
      }
      public void btnCancel_Click(object sender, RoutedEventArgs e)
      {
         Close();
      }

      public void btnOK_Click(object sender, RoutedEventArgs e)
      {
         DialogResult = true;
         _viewModel.TransferParameter();
         Close();
      }
   }
}
