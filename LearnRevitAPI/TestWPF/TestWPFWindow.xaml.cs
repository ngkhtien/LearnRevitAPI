using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace LearnRevitAPI.TestWPF
{
   /// <summary>
   /// Interaction logic for TestWPFWindow.xaml
   /// </summary>
   public partial class TestWPFWindow
   {
      public TestWPFWindow()
      {
         InitializeComponent();
      }

      private void Feedback(object sender, RoutedEventArgs e)
      {
         try
         {
            Process.Start("mailto:contact@alphabimvn.com");
         }
         catch (Exception)
         {
         }
      }
   }
}
