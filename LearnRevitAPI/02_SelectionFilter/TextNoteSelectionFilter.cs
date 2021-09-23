#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
#endregion

namespace LearnRevitAPI._02_SelectionFilter
{
   class TextNoteSelectionFilter : ISelectionFilter
   {
      public bool AllowElement(Element elem)
      {
         string elementName = elem.Category.Name;

         return elementName.Equals("TextNotes");
      }

      public bool AllowReference(Reference reference, XYZ position)
      {
         return true;
      }
   }
}
