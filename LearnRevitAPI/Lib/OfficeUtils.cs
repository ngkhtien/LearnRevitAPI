using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace LearnRevitAPI.Lib
{
   public static class OfficeUtils
   {
      public static string GetPathDialog(string title, string filter = "All files|*.*")
      {
         OpenFileDialog dlg = new OpenFileDialog();
         dlg.Title = title;
         dlg.Filter = filter;

         if (System.Windows.Forms.DialogResult.OK != dlg.ShowDialog())
         {
            MessageBox.Show("Please select file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return String.Empty;
         }
         else
         {
            return dlg.FileName;
         }
      }
   }
}
