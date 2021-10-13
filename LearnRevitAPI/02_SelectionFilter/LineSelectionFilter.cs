#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
#endregion

namespace LearnRevitAPI._02_SelectionFilter
{
   class LineSelectionFilter : ISelectionFilter
   {
      public bool AllowElement(Element elem)
      {
         string elementName = elem.Category.Name;

         return elementName.Equals("Lines");
      }

      public bool AllowReference(Reference reference, XYZ position)
      {
         return true;
      }
   }
}