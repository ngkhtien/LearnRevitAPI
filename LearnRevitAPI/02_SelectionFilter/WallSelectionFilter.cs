#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
#endregion

namespace LearnRevitAPI._02_SelectionFilter
{
   class WallSelectionFilter : ISelectionFilter
   {
      public bool AllowElement(Element elem)
      {
         string elementName = elem.Category.Name;

         return elementName.Equals("Walls");
      }

      public bool AllowReference(Reference reference, XYZ position)
      {
         return true;
      }
   }
}
