using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace LearnRevitAPI.Lib
{
   /// <summary>
   /// Return multiple catagory element
   /// </summary>
   public class EqualityCompareCategory : IEqualityComparer<Element>
   {
      public bool Equals(Element x, Element y)
      {
         return x.Category.Name.Equals(y.Category.Name);
      }

      public int GetHashCode(Element obj)
      {
         return 0;
      }
   }
}
